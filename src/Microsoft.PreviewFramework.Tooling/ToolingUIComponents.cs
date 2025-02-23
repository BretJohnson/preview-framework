using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using System.ComponentModel;

namespace Microsoft.PreviewFramework.Tooling;

public class ToolingUIComponents : UIComponents<ToolingUIComponent, ToolingUIExample>
{
    public void AddFromRoslynCompilation(Compilation compilation)
    {
        // TODO: Handle component categories
        /*
        IEnumerable<UIComponentCategoryAttribute> uiComponentCategoryAttributes = assembly.GetCustomAttributes<UIComponentCategoryAttribute>();
        foreach (UIComponentCategoryAttribute uiComponentCategoryAttribute in uiComponentCategoryAttributes)
        {
            UIComponentCategory category = GetOrAddCatgegory(uiComponentCategoryAttribute.Name);

            foreach (Type type in uiComponentCategoryAttribute.UIComponentTypes)
            {
                UIComponent component = GetOrAddUIComponent(type);
                component.SetCategoryFailIfAlreadySet(category);
            }
        }
        */

        foreach (SyntaxTree syntaxTree in compilation.SyntaxTrees)
        {
            SemanticModel semanticModel = compilation.GetSemanticModel(syntaxTree);

            var exampleWalker = new ExampleWalker(this, semanticModel);

            SyntaxNode root = syntaxTree.GetRoot();
            exampleWalker.Visit(root);
        }

        // TODO: Handle this case
        /*
        UIExampleAttribute? typeExampleAttribute = type.GetCustomAttribute<UIExampleAttribute>(false);
        if (typeExampleAttribute != null)
        {
            AddExample(new ClassUIExample(typeExampleAttribute, type));
        }
        */
    }

    public ToolingUIComponent GetOrAddComponent(string name)
    {
        ToolingUIComponent? component = this.GetUIComponent(name);
        if (component == null)
        {
            component = new ToolingUIComponent(name);
            this.AddUIComponent(component);
        }

        return component;
    }

    public void AddExample(string uiComponentName, ToolingUIExample uiExample)
    {
        ToolingUIComponent component = this.GetOrAddComponent(uiComponentName);
        component.AddExample(uiExample);
    }

    private class ExampleWalker : CSharpSyntaxWalker
    {
        private ToolingUIComponents uiComponents;
        private SemanticModel semanticModel;

        public ExampleWalker(ToolingUIComponents uiComponents, SemanticModel semanticModel)
        {

            this.uiComponents = uiComponents;
            this.semanticModel = semanticModel;
        }

        public override void VisitMethodDeclaration(MethodDeclarationSyntax methodDeclaration)
        {
            this.CheckForUIExample(methodDeclaration);
            base.VisitMethodDeclaration(methodDeclaration);
        }

        private void CheckForUIExample(MethodDeclarationSyntax methodDeclaration)
        {
            AttributeSyntax uiExampleAttribute = methodDeclaration.AttributeLists
                .SelectMany(attrList => attrList.Attributes)
                .FirstOrDefault(attr => attr.Name.ToString() == "UIExample");

            if (uiExampleAttribute == null)
            {
                return;
            }

            var attributeSymbol = this.semanticModel.GetSymbolInfo(uiExampleAttribute).Symbol as IMethodSymbol;
            if (attributeSymbol == null)
            {
                return;
            }

            // Verify that the full qualified name of the attribute is correct
            string fullQualifiedAttributeName = attributeSymbol.ContainingType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            if (fullQualifiedAttributeName != "global::ExampleFramework.UIExampleAttribute")
            {
                return;
            }

            string? uiComponentName = null;
            string? title = null;
            if (uiExampleAttribute.ArgumentList != null)
            {
                SeparatedSyntaxList<AttributeArgumentSyntax> attributeArgs = uiExampleAttribute.ArgumentList.Arguments;

                // If the attribute specifies an example title (1st argument), use it. Otherwise,
                // the title defaults to the method name.
                if (attributeArgs.Count >= 1)
                {
                    AttributeArgumentSyntax firstArgument = attributeArgs[0];
                    if (firstArgument.Expression is LiteralExpressionSyntax literalExpression &&
                        literalExpression.Kind() == SyntaxKind.StringLiteralExpression)
                    {
                        title = literalExpression.Token.ValueText;
                    }
                }

                // If the attribute specifies the UIComponent type, use it. Otherwise, the UIComponent
                // defaults to the method return type
                if (attributeArgs.Count >= 2)
                {
                    AttributeArgumentSyntax secondArgument = attributeArgs[1];
                    if (secondArgument.Expression is TypeOfExpressionSyntax typeOfExpression)
                    {
                        ITypeSymbol? typeSymbol = this.semanticModel.GetTypeInfo(typeOfExpression.Type).Type;
                        if (typeSymbol == null)
                        {
                            return;
                        }

                        uiComponentName = typeSymbol.ToDisplayString();
                    }
                }
            }

            if (uiComponentName == null)
            {
                ITypeSymbol? returnTypeSymbol = this.semanticModel.GetTypeInfo(methodDeclaration.ReturnType).Type;
                if (returnTypeSymbol == null)
                {
                    return;
                }

                uiComponentName = returnTypeSymbol.ToDisplayString();
            }

            if (methodDeclaration.Parent is not TypeDeclarationSyntax typeDeclaration)
            {
                return;
            }

            INamedTypeSymbol? parentTypeSymbol = this.semanticModel.GetDeclaredSymbol(typeDeclaration);
            if (parentTypeSymbol == null)
            {
                return;
            }

            string exampleFullName = $"{parentTypeSymbol.ToDisplayString()}.{methodDeclaration.Identifier.Text}";

            var uiExample = new StaticMethodUIExample(exampleFullName, title);
            this.uiComponents.AddExample(uiComponentName, uiExample);
        }

        public override void VisitClassDeclaration(ClassDeclarationSyntax classDeclaration)
        {
            this.CheckForUIComponent(classDeclaration);
            base.VisitClassDeclaration(classDeclaration);
        }

        private void CheckForUIComponent(ClassDeclarationSyntax classDeclaration)
        {
            if (this.CanBeAutoGeneratedExample(classDeclaration))
            {
                INamedTypeSymbol? classTypeSymbol = this.semanticModel.GetDeclaredSymbol(classDeclaration);
                if (classTypeSymbol == null)
                {
                    return;
                }

                string uiComponentName = classTypeSymbol.ToDisplayString();

                ToolingUIComponent? uiComponent = this.uiComponents.GetUIComponent(uiComponentName);
                if (uiComponent == null || uiComponent.Examples.Count == 0)
                {
                    uiComponent ??= this.uiComponents.GetOrAddComponent(uiComponentName);

                    var uiExample = new ClassUIExample(uiComponentName, isAutoGenerated: true);
                    this.uiComponents.AddExample(uiComponentName, uiExample);
                }
            }
        }

        private bool CanBeAutoGeneratedExample(ClassDeclarationSyntax classDeclaration)
        {
            if (classDeclaration.Modifiers.Any(SyntaxKind.AbstractKeyword))
            {
                return false;
            }

            // Check if the class has a default constructor
            bool hasDefaultConstructor = classDeclaration.Members
                .OfType<ConstructorDeclarationSyntax>()
                .Any(c => c.ParameterList.Parameters.Count == 0 && (c.Modifiers.Any(SyntaxKind.PublicKeyword) || c.Modifiers.Any(SyntaxKind.InternalKeyword)));
            if (!hasDefaultConstructor)
            {
                return false;
            }

            // TODO: Get fully qualified type for ContentPage & do appropriate check
            // for platform
            // Check if the class derives from ContentPage
            bool derivesFromContentPage = classDeclaration.BaseList?
                .Types
                .Any(t => t.Type.ToString() == "ContentPage") ?? false;
            if (!derivesFromContentPage)
            {
                return false;
            }

            return true;
        }
    }
}
