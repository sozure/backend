using Azure.Security.KeyVault.Secrets;
using System.Text.RegularExpressions;
using VGService.Repositories;

namespace VGService;

public class ZService
{
    private readonly KeyVaultConnectionRepository _kvConnectionRepository;
    private readonly Dictionary<string, Dictionary<string, string>> newValues = new()
    {
        ["AiNavigation"] = new()
        {
            ["dev"] = "kaeGhiu7eagif4mo3oNe4pi4Caic3nui",
            ["staging"] = "ApaeCitaet4Puqu9gah7oocieme3cahj",
            ["preprod"] = "vaoghoosakizoj7Aupei4wah9uu9eisi",
        },
        ["Calendar"] = new()
        {
            ["dev"] = "Mu4Che4Eseyiez3eighAeYai4ahquWac",
            ["staging"] = "Punie9ohieMae4zo7oongieNgohCae9s",
            ["preprod"] = "Aive4veiShe7ifaeg3Mahj3seequ3afo",
        },
        ["Configuration"] = new()
        {
            ["dev"] = "yEing3iCh3ah9ou4oquio3noe7Yoh7yu",
            ["staging"] = "iu9mohquai9TieBiti3aRaizaef3Eif7",
            ["preprod"] = "phio9ueh3ueEWetoyaen7eWieKaisugu",
        },
        ["CustomerService"] = new()
        {
            ["dev"] = "Eej9ahb4Ho3ecaisoXv3ohphae3aewah",
            ["staging"] = "eex9eishua9dah9wohphoo3AhxaeshaC",
            ["preprod"] = "yoh7ahh4phaeceo7Thoovej7chush7ph",
        },
        ["Definition"] = new()
        {
            ["dev"] = "mi7daingae7oothik4Uuj3dae3Todief",
            ["staging"] = "Aepaec3yaeHaiK9aehephei9Ajiethei",
            ["preprod"] = "shoh7upaadie9binahJ7oot7XieLae9c",
        },
        ["Faq"] = new()
        {
            ["dev"] = "au4ieg4ku9eiMeib7thoh4aicoafoZxe",
            ["staging"] = "nee3ic9ohrah4Keizu7niexee7weew9p",
            ["preprod"] = "oT9hied4ooPuyoo9ia7oe3meeve4shae",
        },
        ["Floorplan"] = new()
        {
            ["dev"] = "aHfohEo5Rxaingon9haeWa3Eitei9hei",
            ["staging"] = "re7cu3ceiw9ko3aepheNgah9aeMeekov",
            ["preprod"] = "Doj7aivaePhe9eu9phe4ciethof3eihu",
        },
        ["GeoData"] = new()
        {
            ["dev"] = "teeTiekubeg3Teixooth9ooLiech7Yuu",
            ["staging"] = "aenai4apho7ifei3AhCheeboyohvoox9",
            ["preprod"] = "ieLoh7wui9iegaisae9wa3re4tueveez",
        },
        ["Identity"] = new()
        {
            ["dev"] = "mNophie3aigai7aes9eehi3eighe7Tae",
            ["staging"] = "aivahj4eeC3ahvoti3Mi9cie7vahghee",
            ["preprod"] = "aa9cogohzaeF9wich3fagai9oodaiw9a",
        },
        ["InternalLog"] = new()
        {
            ["dev"] = "pegeiPhu9aem4bioVeicHa6mahthiu9a",
            ["staging"] = "acahpee3EWoo7koh3goo9ohphieRee9m",
            ["preprod"] = "ohMivooFaexaeph9rae7Ane3wiecheeT",
        },
        ["Location"] = new()
        {
            ["dev"] = "ub7faev7Ae9Teg4thohy3dae3uCahchu",
            ["staging"] = "io7houN3aec7goh9nei9Woo7ahsoshai",
            ["preprod"] = "yeih4mi4ahhoh4pheeCifoo9ahthah9i",
        },
        ["LocationSharingManager"] = new()
        {
            ["dev"] = "ahp4Eigi7eengaithoz7couH9eiquaeR",
            ["staging"] = "exLomeiw3mUophaw4oX3niebui9uit3i",
            ["preprod"] = "Joo3iechah7ahqu7xieCaighi7ohch3s",
        },
        ["LogCollector"] = new()
        {
            ["dev"] = "gohfahgueNg7eepi4ooghah9fohchah9",
            ["staging"] = "iob4iw3quohch9ois9NiecheeThos3ua",
            ["preprod"] = "EaW3bighohjaoc7shiezunge4rahquee",
        },
        ["MeetingRoom"] = new()
        {
            ["dev"] = "ehohJ9maofif9aepitee9Xoo9dohpieb",
            ["staging"] = "eekir4eeyej9che7peijaipoh3do3Yai",
            ["preprod"] = "zEWuom3epe9thohPaqTh7uUch3xuacah",
        },
        ["Notification"] = new()
        {
            ["dev"] = "bohheing3eef9Pei4kithaewi3pheecu",
            ["staging"] = "rahjeiLi9MohgheiThitoox3ais7Ahvi",
            ["preprod"] = "peiP3usoathaefach9soa7hahseeRaip",
        },
        ["Person"] = new()
        {
            ["dev"] = "eadu3Theob9jaepohnaipaeb3qui4ou4",
            ["staging"] = "weiboh4oniePh7sheimoh4Aec9chei3a",
            ["preprod"] = "einGahhaithach7gee9ieth4urazeuPa",
        },
        ["PresenceReport"] = new()
        {
            ["dev"] = "ohNg4yah4saa9jow7ienu7ua3cobae4J",
            ["staging"] = "chah7xi3woh3iyooThai7yox7utiesha",
            ["preprod"] = "uh3iepheepaiwoh9ba7neeChahsi4Xu3",
        },
        ["Privacy"] = new()
        {
            ["dev"] = "ea7AeFaucie9aiXop3shaa4aezieph9k",
            ["staging"] = "Dhu3fi7ooci7phineiriJ9pahhe3cei7",
            ["preprod"] = "ohdah9geipahraix4neicaeRaegooTee",
        },
        ["Scheduler"] = new()
        {
            ["dev"] = "iechaeJie7cheich3ahbahchahquei9j",
            ["staging"] = "Rechaqu4oozahn7pei3iequeeYutai9i",
            ["preprod"] = "ra3xeicheacoo7Aeshai9aKaiH4ivaec",
        },
        ["SmartParking"] = new()
        {
            ["dev"] = "iud4os9aePhoh4Chaocei9aezeiH3shi",
            ["staging"] = "thie9neit4tiebeikiefah9sieng4Yin",
            ["preprod"] = "phoc4zoo9oyohgh9Cahcae3pooquoNgu",
        },
        ["Storage"] = new()
        {
            ["dev"] = "oeghAinEiVoongae3Coo3ae7kahk3Fa3",
            ["staging"] = "VooJiquTow7ohMe9oTeiqu9yug3eni4e",
            ["preprod"] = "xaeCee4xahtie3uv3Naph4uNg3mohach",
        },
    };

    public ZService(KeyVaultConnectionRepository keyVaultConnectionRepository)
    {
        _kvConnectionRepository = keyVaultConnectionRepository;
    }

    public async Task RunChange(string environment)
    {
        var secrets = await _kvConnectionRepository.GetKeyVaultSecrets();
        var filteredSecrets = Filter(secrets!, "MsSql");
        Console.WriteLine($"Environment:{environment}");

        foreach (var secret in filteredSecrets)
        {
            var msName = secret.Name.Split("--")[0].Replace("api", "").Replace("Api", "");
            msName = msName == "LogCollector" ? "InternalLog" : msName;

            if (newValues.ContainsKey(msName) || msName == "Identityserver")
            {
                // A LogCollector esetén az InternalLog adatait kapja fel a map-ből.
                var passwords = newValues[msName == "Identityserver" ? "Identity" : msName];
                var password = passwords[environment];
                var newConnectionString = $"Data Source=mssql-{environment}.db,1433;Initial Catalog={msName};User id={msName.ToLower() + "_usr"};Password={password}";

                var newSecret = new Dictionary<string, string>()
                {
                    ["secretName"] = secret.Name,
                    ["secretValue"] = newConnectionString
                };

                await _kvConnectionRepository.AddKeyVaultSecret(newSecret);
                Console.WriteLine($"ConnectionString changed: {msName}.");
            }
        }
    }

    public async Task CheckChange(string environment)
    {
        var secrets = await _kvConnectionRepository.GetKeyVaultSecrets();
        var filteredSecrets = Filter(secrets!, "MsSql");
        var result = "ok";

        foreach (var secret in filteredSecrets)
        {
            var msName = secret.Name.Split("--")[0].Replace("api", "").Replace("Api", "");
            msName = msName == "LogCollector" ? "InternalLog" : msName;

            if (newValues.ContainsKey(msName) || msName == "Identityserver")
            {
                var passwords = newValues[msName == "Identityserver" ? "Identity" : msName];
                var password = passwords[environment];
                var goodConnString = $"Data Source=mssql-{environment}.db,1433;Initial Catalog={msName};User id={msName.ToLower() + "_usr"};Password={password}";

                if (secret.Value != goodConnString)
                {
                    result = "not ok";
                    break;
                }
            }
        }

        Console.WriteLine($"Environment: {environment}, list is {result}");
    }

    protected static IEnumerable<KeyVaultSecret> Filter(IEnumerable<KeyVaultSecret> keyVaultSecrets, string filter)
    {
        var regex = new Regex(filter);
        return keyVaultSecrets.Where(secret => regex.IsMatch(secret?.Name ?? ""));
    }
}
