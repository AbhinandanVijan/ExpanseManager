using System.Security.Cryptography;
using System.Text;
using ExpanseManagerAPI.Interfaces.Security;

namespace ExpanseManagerAPI.Security;

// NOTE: For learning purposes. In production use a proper key store (Azure Key Vault, AWS KMS, etc.)
public class AesTokenEncryptionService : ITokenEncryptionService
{
    private readonly byte[] _key;
    private readonly byte[] _iv;

    public AesTokenEncryptionService(IConfiguration config)
    {
        // Put these in appsettings.json as Base64 strings
        _key = Convert.FromBase64String(config["Crypto:AesKey"]!);
        _iv  = Convert.FromBase64String(config["Crypto:AesIv"]!);
    }

    public string Encrypt(string plaintext)
    {
        using var aes = Aes.Create();
        aes.Key = _key;
        aes.IV = _iv;

        using var encryptor = aes.CreateEncryptor();
        var plainBytes = Encoding.UTF8.GetBytes(plaintext);
        var cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
        return Convert.ToBase64String(cipherBytes);
    }

    public string Decrypt(string ciphertext)
    {
        using var aes = Aes.Create();
        aes.Key = _key;
        aes.IV = _iv;

        using var decryptor = aes.CreateDecryptor();
        var cipherBytes = Convert.FromBase64String(ciphertext);
        var plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
        return Encoding.UTF8.GetString(plainBytes);
    }
}
