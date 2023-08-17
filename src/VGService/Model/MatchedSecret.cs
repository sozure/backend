namespace VGService.Model;

public class MatchedSecret
{
    public string SecretName { get; set; }
    public string SecretValue { get; set; }
    public string CreatedBy { get; set; }

    public MatchedSecret(string secretName, string secretValue)
    {
        SecretName = secretName;
        SecretValue = secretValue;
    }
}
