// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using ExampleFramework.Tooling;
using Microsoft.TestPlatform.AdapterUtilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace ExampleFramework.TestAdapter.ObjectModel;

/// <summary>
/// The unit test element.
/// </summary>
[Serializable]
internal class TestElement
{
    public TestElement(UIComponent uiComponent, string source)
    {
        UIComponent = uiComponent;
        Source = source;
    }

    public UIComponent UIComponent { get; }

    public string Source { get; }

    public TestCase ToTestCase(UIExample uiExample)
    {
        // This causes compatibility problems with older runners.
        // string testFullName = this.TestMethod.HasManagedMethodAndTypeProperties
        //                 ? string.Format(CultureInfo.InvariantCulture, "{0}.{1}", this.TestMethod.ManagedTypeName, this.TestMethod.ManagedMethodName)
        //                 : string.Format(CultureInfo.InvariantCulture, "{0}.{1}", this.TestMethod.FullClassName, this.TestMethod.Name);
        var testFullName = uiExample.FullName;

        var testCase = new TestCase(testFullName, Constants.ExecutorUri, Source)
        {
            DisplayName = uiExample.Title,
        };


#if false
        if (UIExample.HasManagedMethodAndTypeProperties)
        {
            testCase.SetPropertyValue(TestCaseExtensions.ManagedTypeProperty, UIExample.ManagedTypeName);
            testCase.SetPropertyValue(TestCaseExtensions.ManagedMethodProperty, UIExample.ManagedMethodName);
            testCase.SetPropertyValue(Constants.TestClassNameProperty, UIExample.ManagedTypeName);
        }
        else
        {
            testCase.SetPropertyValue(Constants.TestClassNameProperty, UIExample.FullClassName);
        }
        */

        var hierarchy = UIExample.Hierarchy;
        if (hierarchy != null && hierarchy.Count > 0)
        {
            testCase.SetHierarchy(hierarchy.ToArray());
        }

        // Set declaring type if present so the correct method info can be retrieved
        if (UIExample.DeclaringClassFullName != null)
        {
            testCase.SetPropertyValue(Constants.DeclaringClassNameProperty, UIExample.DeclaringClassFullName);
        }

        // Many of the tests will not be async, so there is no point in sending extra data
        if (IsAsync)
        {
            testCase.SetPropertyValue(Constants.AsyncTestProperty, IsAsync);
        }

        // Set only if some test category is present
        if (TestCategory != null && TestCategory.Length > 0)
        {
            testCase.SetPropertyValue(Constants.TestCategoryProperty, TestCategory);
        }

        // Set priority if present
        if (Priority != null)
        {
            testCase.SetPropertyValue(Constants.PriorityProperty, Priority.Value);
        }

        if (Traits != null)
        {
            testCase.Traits.AddRange(Traits);
        }

        if (!string.IsNullOrEmpty(CssIteration))
        {
            testCase.SetPropertyValue(Constants.CssIterationProperty, CssIteration);
        }

        if (!string.IsNullOrEmpty(CssProjectStructure))
        {
            testCase.SetPropertyValue(Constants.CssProjectStructureProperty, CssProjectStructure);
        }

        if (!string.IsNullOrEmpty(Description))
        {
            testCase.SetPropertyValue(Constants.DescriptionProperty, Description);
        }

        if (WorkItemIds != null)
        {
            testCase.SetPropertyValue(Constants.WorkItemIdsProperty, WorkItemIds);
        }

        // The list of items to deploy before running this test.
        if (DeploymentItems != null && DeploymentItems.Length > 0)
        {
            testCase.SetPropertyValue(Constants.DeploymentItemsProperty, DeploymentItems);
        }

        // Set the Do not parallelize state if present
        if (DoNotParallelize)
        {
            testCase.SetPropertyValue(Constants.DoNotParallelizeProperty, DoNotParallelize);
        }

        // Store resolved data if any
        if (UIExample.DataType != DynamicDataType.None)
        {
            var data = UIExample.SerializedData;

            testCase.SetPropertyValue(Constants.TestDynamicDataTypeProperty, (int)UIExample.DataType);
            testCase.SetPropertyValue(Constants.TestDynamicDataProperty, data);
        }
#endif

        testCase.Id = GenerateTestId(uiExample);

        return testCase;
    }

    private Guid GenerateTestId(UIExample uiExample)
    {
        var idProvider = new TestIdProvider();

        idProvider.AppendString(Constants.ExecutorUriString);

        // Below comment is copied over from Test Platform.
        // If source is a file name then just use the filename for the identifier since the file might have moved between
        // discovery and execution (in appx mode for example). This is not elegant because the Source contents should be
        // a black box to the framework.
        // For example in the database adapter case this is not a file path.
        // As discussed with team, we found no scenario for netcore, & fullclr where the Source is not present where ID
        // is generated, which means we would always use FileName to generate ID. In cases where somehow Source Path
        // contained garbage character the API Path.GetFileName() we are simply returning original input.
        // For UWP where source during discovery, & during execution can be on different machine, in such case we should
        // always use Path.GetFileName().
        string fileNameOrFilePath = Source;
        try
        {
            fileNameOrFilePath = Path.GetFileName(fileNameOrFilePath);
        }
        catch (ArgumentException)
        {
            // In case path contains invalid characters.
        }

        idProvider.AppendString(fileNameOrFilePath);
        idProvider.AppendString(uiExample.FullName);

        return idProvider.GetId();
    }
}

