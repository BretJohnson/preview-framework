using ImageMagick;

namespace VisualTestUtils.MagickNet
{
    public class MagickNetImageEditor : IImageEditor
    {
        MagickImage magickImage;

        public MagickNetImageEditor(ImageSnapshot imageSnapshot)
        {
            this.magickImage = new MagickImage(imageSnapshot.Data);
        }

        public void Crop(int x, int y, int width, int height)
        {
            this.magickImage.Crop(new MagickGeometry(x, y, width, height));
            this.magickImage.RePage();
        }

        public (int width, int height) GetSize() =>
            (this.magickImage.Width, this.magickImage.Height);

        public ImageSnapshot GetUpdatedImage()
        {
            ImageSnapshotFormat format = this.magickImage.Format switch
            {
                MagickFormat.Png => ImageSnapshotFormat.PNG,
                MagickFormat.Jpeg => ImageSnapshotFormat.JPEG,
                _ => throw new NotSupportedException($"Unexpected image format: {this.magickImage.Format}")
            };

            return new ImageSnapshot(this.magickImage.ToByteArray(), format);
        }
    }
}
