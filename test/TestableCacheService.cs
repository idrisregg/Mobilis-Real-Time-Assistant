
namespace Mobilis_Real_Time_Assistant.Test
{
    public class TestableContextCacheService
    {
        private readonly string _filePath;

        public TestableContextCacheService(string filePath)
        {
            _filePath = filePath;
        }

        public async Task<string> GetContextAsync()
        {
            return await File.ReadAllTextAsync(_filePath);
        }
    }
}
