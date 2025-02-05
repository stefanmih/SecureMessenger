using Xunit;
using SecureMessenger.Infrastructure.Services;

public class AesEncryptionServiceTests
{
    private readonly AesEncryptionService _aesEncryptionService;

    public AesEncryptionServiceTests()
    {
        _aesEncryptionService = new AesEncryptionService();
    }

    [Fact]
    public void Encrypt_ShouldEncryptAndDecryptCorrectly()
    {
        string originalMessage = "Hello World!";
        string password = "testpassword";

        string encryptedMessage = _aesEncryptionService.Encrypt(originalMessage, password);
        string decryptedMessage = _aesEncryptionService.Decrypt(encryptedMessage, password);

        Assert.Equal(originalMessage, decryptedMessage);
    }

    [Fact]
    public void Decrypt_ShouldReturnNullOnInvalidKey()
    {
        string originalMessage = "Hello World!";
        string password = "testpassword";
        string wrongPassword = "wrongpassword";

        string encryptedMessage = _aesEncryptionService.Encrypt(originalMessage, password);
        string decryptedMessage = _aesEncryptionService.Decrypt(encryptedMessage, wrongPassword);

        Assert.NotEqual(originalMessage, decryptedMessage);
    }
}
