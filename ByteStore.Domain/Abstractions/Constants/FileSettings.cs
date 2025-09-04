namespace ByteStore.Domain.Abstractions.Constants
{
    public static class FileSettings
    {
        public const string ImagePath = "assets/images/games";
        public static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".webp"];
        public const int MaxFileSizeInMB = 5;
        public const int MaxFileSizeInBytes = MaxFileSizeInMB * 1024 * 1024;
    }
}
