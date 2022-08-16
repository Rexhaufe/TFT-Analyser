using System.Net.Http.Headers;
using TFT_Data_Analyser;
using Newtonsoft.Json;

const string path = @"C:\Users\Anwender\Desktop\TFT Data Analyser\";
const string apiKey = "RGAPI-3b3d27af-312a-454c-bc4d-28392b81567e";
int count = 20;
Boolean writeProcessInConsole = false;
int processNumber = 0;
ProgressBar progressBar;
List<PlayerProfile> selectedPlayerProfile;
List<Match> selectedMatches;

//Dictionary setups
Dictionary<string, int> traitDictionary = new Dictionary<string, int>();
Dictionary<string, int> unitDictionary = new Dictionary<string, int>();
List<string> matchIDList = new List<string>();


Console.Write("Do you want to load Matches from a file? y/n : ");
if (Console.ReadLine() == "y")
{
    Console.WriteLine("Choose filename to load from: ");
    for (int i = 0; i < Directory.GetFiles(path).Length; i++)
    {
        Console.WriteLine(i + ": " + Directory.GetFiles(path)[i].Substring(path.Length));
    }

    while (true)
    {
        string input = Console.ReadLine();
        try
        {
            if (input != null && Int16.Parse(input) >= 0 && Int16.Parse(input) < Directory.GetFiles(path).Length)
            {
                selectedMatches = loadMatchList(path, Directory.GetFiles(path)[Int16.Parse(input)].Substring(path.Length));
                Console.WriteLine("Loaded in " + selectedMatches.Count + " Matches");
                break;
            }
        }
        catch (FormatException)
        {
            Console.WriteLine("Only Numbers are allowed");
            continue;
        }
        Console.WriteLine("file not found. Please try again");
    }
}
else //in the case he doesnt want to load matches, he may wants to load profiles and new matches
{
    Console.Write("Do you want to load Profiles from a file? y/n : ");

    if (Console.ReadLine() == "n")
    {
        Console.Write("How many Games per Player do you want to load? ");
        count = Int16.Parse(Console.ReadLine());
        Console.WriteLine(count);
        List<PlayerProfile> challengerPlayerProfiles = getAPIProfiles("https://euw1.api.riotgames.com/tft/league/v1/challenger");
        List<PlayerProfile> grandmasterPlayerProfiles = getAPIProfiles("https://euw1.api.riotgames.com/tft/league/v1/grandmaster");
        List<PlayerProfile> masterPlayerProfiles = getAPIProfiles("https://euw1.api.riotgames.com/tft/league/v1/master");
        List<PlayerProfile> diamond1PlayerProfiles = getAPIProfiles("https://euw1.api.riotgames.com/tft/league/v1/entries/DIAMOND/I");
        List<PlayerProfile> diamond2PlayerProfiles = getAPIProfiles("https://euw1.api.riotgames.com/tft/league/v1/entries/DIAMOND/II");
        List<PlayerProfile> diamond3PlayerProfiles = getAPIProfiles("https://euw1.api.riotgames.com/tft/league/v1/entries/DIAMOND/III");
        List<PlayerProfile> diamond4PlayerProfiles = getAPIProfiles("https://euw1.api.riotgames.com/tft/league/v1/entries/DIAMOND/IV");
        List<PlayerProfile> allDiamondPlayerProfiles = diamond1PlayerProfiles.Concat(diamond2PlayerProfiles).Concat(diamond3PlayerProfiles).Concat(diamond4PlayerProfiles).ToList();
        List<PlayerProfile> allPlayerProfiles = challengerPlayerProfiles.Concat(grandmasterPlayerProfiles).Concat(masterPlayerProfiles).Concat(allDiamondPlayerProfiles).ToList();
        List<List<PlayerProfile>> listOfProfiles = new List<List<PlayerProfile>>();
        listOfProfiles.Add(challengerPlayerProfiles);
        listOfProfiles.Add(grandmasterPlayerProfiles);
        listOfProfiles.Add(masterPlayerProfiles);
        listOfProfiles.Add(diamond1PlayerProfiles);
        listOfProfiles.Add(diamond2PlayerProfiles);
        listOfProfiles.Add(diamond3PlayerProfiles);
        listOfProfiles.Add(diamond4PlayerProfiles);
        listOfProfiles.Add(allDiamondPlayerProfiles);
        listOfProfiles.Add(allPlayerProfiles);
        Console.WriteLine("================================================");
        Console.WriteLine("Which profiles do you want to use?");
        Console.WriteLine("0: challengerPlayerProfiles");
        Console.WriteLine("1: grandmasterPlayerProfiles");
        Console.WriteLine("2: masterPlayerProfiles");
        Console.WriteLine("3: diamond1PlayerProfiles");
        Console.WriteLine("4: diamond2PlayerProfiles");
        Console.WriteLine("5: diamond3PlayerProfiles");
        Console.WriteLine("6: diamond4PlayerProfiles");
        Console.WriteLine("7: allDiamondPlayerProfiles");
        Console.WriteLine("8: allPlayerProfiles");
        while (true)
        {
            try
            {
                selectedPlayerProfile = listOfProfiles[Int16.Parse(Console.ReadLine())];
                break;
            }
            catch
            {
                Console.WriteLine("list not found, please try again");
            }
        }
        Console.WriteLine("All Player Profiles found: " + selectedPlayerProfile.Count);
        addPUUIDtoPlayerList(selectedPlayerProfile);
        addMatchidsToPlayerProfiles(selectedPlayerProfile, count);
        while (true)
        {
            Console.Write("Do you want to save the new Profiles? y/n : ");
            string input = Console.ReadLine();
            //TODO if input is null take the default one, otherwise accept a custom name
            if (input == "y")
            {
                Console.Write("Filename: ");
                string filename = Console.ReadLine();
                if(filename == null) saveProfileList(selectedPlayerProfile, path, "Profile_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".json");
                else saveProfileList(selectedPlayerProfile, path, filename + ".json");
                break;
            }
            else break;
        }
        selectedMatches = getAPIMatches(selectedPlayerProfile);
        while (true)
        {
            Console.Write("Do you want to save the new Match List? y/n : ");
            string input = Console.ReadLine();
            //TODO if input is null take the default one, otherwise accept a custom name
            if (input == "y")
            {
                Console.Write("Filename: ");
                string filename = Console.ReadLine();
                if(filename == null) saveMatchList(selectedMatches, path, "MatchList_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".json");
                else saveMatchList(selectedMatches, path, filename + ".json");
                break;
            }
            else break;
        }
    } //if he wants to load new Profiles from API
    else
    {
        Console.WriteLine("Choose filename to load from: ");
        for (int i = 0; i < Directory.GetFiles(path).Length; i++)
        {
            Console.WriteLine(i + ": " + Directory.GetFiles(path)[i].Substring(path.Length));
        }

        while (true)
        {
            string input = Console.ReadLine();
            try
            {
                if (input != null && Int16.Parse(input) >= 0 && Int16.Parse(input) < Directory.GetFiles(path).Length)
                {
                    selectedPlayerProfile = loadProfileList(path, Directory.GetFiles(path)[Int16.Parse(input)].Substring(path.Length));
                    Console.WriteLine("Loaded in " + selectedPlayerProfile.Count + " Profiles");
                    goto endOfLoop;
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Only Numbers are allowed");
                continue;
            }
            Console.WriteLine("file not found. Please try again");
        }
    endOfLoop:
        Console.WriteLine("Start Loading Matches from Profiles");
        selectedMatches = getAPIMatches(selectedPlayerProfile);
        while (true)
        {
            Console.Write("Do you want to save the new Match List? y/n : ");
            string input = Console.ReadLine();
            //TODO if input is null take the default one, otherwise accept a custom name
            if (input == "y")
            {
                Console.Write("Filename: ");
                string filename = Console.ReadLine();
                if(filename == null) saveMatchList(selectedMatches, path, "MatchList_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".json");
                else saveMatchList(selectedMatches, path, filename + ".json");
                break;
            }
            else break;
        }
    } //if he wants to reload Profiles and Matches from a file
}

List<Player> winnerPlayers = new List<Player>();
System.Console.WriteLine(selectedMatches.Count);
foreach (Match match in selectedMatches)
{
    if (matchIDList.Contains(match.matchId)) continue;
    matchIDList.Add(match.matchId);
    foreach (Player player in match.participants)
    {
        if (player.placement <= 2) 
        {
            winnerPlayers.Add(player);
        }
    }
}

System.Console.WriteLine(winnerPlayers.Count);

foreach(Player player in winnerPlayers)
{
    Trait mainTrait = new Trait("", 0, 0, 0, 0);
    foreach(Trait trait in player.traits)
    {
        if (trait.tier_current > mainTrait.tier_current) mainTrait = trait;
    }
    if (traitDictionary.ContainsKey(mainTrait.name))
    {
        traitDictionary[mainTrait.name]++;
    }
    else
    {
        traitDictionary.Add(mainTrait.name, 0);
        System.Console.WriteLine("Added Trait " + mainTrait.name + " to the Trait Dictionary");
    }
}

var sortedTraitDict = from entry in traitDictionary orderby entry.Value descending select entry;
for (int i = 0; i < sortedTraitDict.Count(); i++)
{
    Console.WriteLine("The {0} used main Trait is {1} and was used {2} times", i + 1, sortedTraitDict.ElementAt(i).Key, sortedTraitDict.ElementAt(i).Value);
}


//Functions

List< Match > loadMatchList(string path, string filename)
{
    return JsonConvert.DeserializeObject<List<Match>>(File.ReadAllText(path + filename));
}

void saveMatchList(List<Match> matchList, string path, string filename)
{
    File.WriteAllText(path + filename, JsonConvert.SerializeObject(matchList));
    Console.WriteLine("Match List successfully saved!");
}

List<PlayerProfile> loadProfileList(string path, string filename)
{
    return JsonConvert.DeserializeObject<List<PlayerProfile>>(File.ReadAllText(path + filename));
}

void saveProfileList(List<PlayerProfile> profileList, string path, string filename)
{
    File.WriteAllText(path + filename, JsonConvert.SerializeObject(profileList));
    Console.WriteLine("ProfileList successfully saved!");
}

void addMatchidsToPlayerProfiles(List<PlayerProfile> listOfPlayerProfiles, int count)
{
    string URL = "https://europe.api.riotgames.com/tft/match/v1/matches/by-puuid/";
    string urlParameters = "ids?api_key=" + apiKey + "&count=" + count;
    processNumber = 0;
    writeProcessInConsole = true;
    returnProcessStatus("Adding Match ID's to Player Profiles Process", (float)listOfPlayerProfiles.Count);
    foreach (PlayerProfile playerProfile in listOfPlayerProfiles)
    {
        processNumber++;
        HttpClient client = new HttpClient();
        //Add an Accept header for JSON format
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.BaseAddress = new Uri(URL + playerProfile.puuid + "/");

        // List data response.
        HttpResponseMessage response;
        while (true)
        {
            response = client.GetAsync(urlParameters).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
            if (response.StatusCode != System.Net.HttpStatusCode.TooManyRequests) break;
            else
            {
                tooManyRequestsMessage(30);
            }
        }

        if (response.IsSuccessStatusCode)
        {
            string json = response.Content.ReadAsStringAsync().Result;
            for (int i = 0; true; i++)
            {
                if (json[i] == ']') break;
                if (json[i] == '"')
                {
                    int matchidlength;
                    for(int j = 1; true; j++)
                    {
                        if(json[i + j] == '"')
                        {
                            matchidlength = j - 1;
                            break;
                        }
                    }
                    playerProfile.matchids.Add(json.Substring(i + 1, matchidlength));
                    json = json.Substring(i + matchidlength + 1);
                    i = 0;
                }
            }
            client.Dispose();
        }
        else
        {
            Console.WriteLine("HTTP Request failed at Adding Match IDS to Player Profiles");
            Console.WriteLine("{0} ({1}) at summoner Name: {2} | ID: {3} ", (int)response.StatusCode, response.ReasonPhrase, playerProfile.summonerName, playerProfile.summonerId);
            Console.WriteLine("Request that failed: " + client.BaseAddress + urlParameters);
            return;
        }
    }
    writeProcessInConsole = false;
    progressBar.Dispose();
    Console.WriteLine("... done");
    processNumber = 0;
}

void addPUUIDtoPlayerList(List<PlayerProfile> listOfPlayerProfiles)
{
    string URL = "https://euw1.api.riotgames.com/tft/summoner/v1/summoners/";
    string urlParameters = "?api_key=" + apiKey;
    writeProcessInConsole = true;
    returnProcessStatus("Adding PUUID to Players Process", (float)listOfPlayerProfiles.Count);
    foreach (PlayerProfile playerProfile in listOfPlayerProfiles)
    {
        processNumber++;
        HttpClient client = new HttpClient();
        //Add an Accept header for JSON format
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.BaseAddress = new Uri(URL + playerProfile.summonerId);

        // List data response.
        HttpResponseMessage response;
        while (true)
        {
            response = client.GetAsync(urlParameters).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
            if (response.StatusCode != System.Net.HttpStatusCode.TooManyRequests) break;
            else
            {
                tooManyRequestsMessage(30);
            }
        }

        if (response.IsSuccessStatusCode)
        {
            string json = response.Content.ReadAsStringAsync().Result;
            for(int i = 0; i < json.Length - 4; i++)
            {
                if(json.Substring(i, 5) == "puuid")
                {
                    int puuidLength = 0;
                    for (int j = 0; true; j++)
                    {
                        if (json[j + i + 8] == '"')
                        {
                            puuidLength = j;
                            break;
                        }
                    }
                    playerProfile.puuid = json.Substring(i + 8, puuidLength);
                    break;
                }
            }
            client.Dispose();
        }
        else
        {
            Console.WriteLine("HTTP Request failed at Adding PUUIDS to Player Profiles");
            Console.WriteLine("{0} ({1}) at summoner Name: {2} | ID: {3} ", (int)response.StatusCode, response.ReasonPhrase, playerProfile.summonerName, playerProfile.summonerId);
            Console.WriteLine("Request that failed: " + client.BaseAddress + urlParameters);
            return;
        }
    }
    writeProcessInConsole = false;
    progressBar.Dispose();
    Console.WriteLine("... done");
    processNumber = 0;
}

List<PlayerProfile> getAPIProfiles(string URL) {
    string urlParameters = "?api_key=" + apiKey + "&?count=" + count;

    HttpClient client = new HttpClient();
    client.BaseAddress = new Uri(URL);

    //Add an Accept header for JSON format
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

    // List data response.
    HttpResponseMessage response;
    while (true)
    {
        response = client.GetAsync(urlParameters).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
        if (response.StatusCode != System.Net.HttpStatusCode.TooManyRequests) break;
        else
        {
            Console.WriteLine("Too Many Requests! Waiting for 30 second");
            Thread.Sleep(30000);
            Console.WriteLine("Continuing");
        }
    }
    string json;

    if (response.IsSuccessStatusCode)
    {
        json = response.Content.ReadAsStringAsync().Result;
        json.Remove(json.Length - 1); //delete last character (a curly bracket) because it makes serializiation easier
    }
    else
    {
        Console.WriteLine("HTTP Request failed at getting Player Profiles");
        Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase); 
        Console.WriteLine("Request that failed: " + client.BaseAddress + urlParameters);
        return null;
    }

    //deleting start of the json to make seraliziation easier
    try
    {
        for (int i = 0; true; i++)
        {

            if (json.Substring(i, 7) == "entries")
            {
                json = json.Substring(i + 10);
                break;
            }
        }
    }
    catch
    {
        json = json.Substring(1);
    }

    List<PlayerProfile> listOfPlayerProfiles = new List<PlayerProfile>();

    for (int i = 0; i < json.Length; i++)
    {
        if (json[i] == '}')
        {
            String jsonClass = json.Substring(0, i + 1);
            PlayerProfile playerProfile = JsonConvert.DeserializeObject<PlayerProfile>(jsonClass);
            listOfPlayerProfiles.Add(playerProfile);
            json = json.Substring(i + 2);
            i = 0;
        }
    }
    Console.WriteLine("Found: " + listOfPlayerProfiles.Count + " Profiles on " + URL);
    return listOfPlayerProfiles;
}

List<Match> getAPIMatches(List<PlayerProfile> listOfPlayerProfiles)
{
    const string URL = "https://europe.api.riotgames.com/tft/match/v1/matches/";
    string urlParameters = "?api_key=" + apiKey;
    List<Match> listOfMatches = new List<Match>();

    processNumber = 0;
    int amountOfMatches = 0;
    foreach(PlayerProfile profile in listOfPlayerProfiles)
    {
        amountOfMatches += profile.matchids.Count;
    }

    writeProcessInConsole = true;
    returnProcessStatus("Get Matches Process", (float)amountOfMatches);

    foreach (PlayerProfile playerProfile in listOfPlayerProfiles)
    {
        foreach (string matchId in playerProfile.matchids)
        {
            processNumber++;
            HttpClient client = new HttpClient();
            //Add an Accept header for JSON format
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.BaseAddress = new Uri(URL + matchId);

            // List data response.
            HttpResponseMessage response;
            while (true)
            {
                response = client.GetAsync(urlParameters).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
                if (response.StatusCode != System.Net.HttpStatusCode.TooManyRequests) break;
                else
                {
                    tooManyRequestsMessage(30);
                }
            }

            if (response.IsSuccessStatusCode)
            {
                string json = response.Content.ReadAsStringAsync().Result;
                //delete start for deserializing
                json = json.Remove(1, 31);
                int deleteRange = 0;
                //delete something in the middle for deserializing
                for (int i = 0; true; i++)
                {
                    if (json[14 + matchId.Length + i] == '{')
                    {
                        deleteRange = i;
                        break;
                    }
                }
                json = json.Remove(16 + matchId.Length, deleteRange);
                //delete something in the end for deserializing
                deleteRange = 0;
                for(int i = 0; true ; i++)
                {
                    if(json[json.Length - 2 - i] ==  ']')
                    {
                        deleteRange = i;
                        break;
                    }
                }
                json = json.Remove(json.Length - 1 - deleteRange, deleteRange);
                listOfMatches.Add(JsonConvert.DeserializeObject<Match>(json));
                client.Dispose();
            }
            else
            {
                Console.WriteLine("HTTP Request failed loading Matches from MatchIDS");
                Console.WriteLine("{0} ({1}) at summoner Name: {2} | ID: {3} | MatchID: {4}", (int)response.StatusCode, response.ReasonPhrase, playerProfile.summonerName, playerProfile.summonerId, matchId);
                Console.WriteLine("Request that failed: " + client.BaseAddress + urlParameters);
                return null;
            }
        }
    }
    writeProcessInConsole = false;
    progressBar.Dispose();
    Console.WriteLine("... done");
    processNumber = 0;
    return listOfMatches;
}

async void returnProcessStatus(string processName, float max)
{
    Console.WriteLine("Starting the {0} which contains {1} processes", processName, (int)max);
    progressBar = new ProgressBar();
    while (writeProcessInConsole)
    {
        progressBar.Report((double)processNumber / max);
        if (((float)processNumber / max) * 100 == 100) break;
        await Task.Delay(250);
    }
}

void writeBeforeProgressBarLine(string text)
{
    progressBar.Dispose();
    Console.SetCursorPosition(0, Console.CursorTop - 0);
    Console.Write("                                                     ");
    Console.SetCursorPosition(0, Console.CursorTop - 0);
    Console.WriteLine(text);
    double tempProgress = progressBar.getCurrentProgress();
    progressBar = new ProgressBar(tempProgress);
}

void tooManyRequestsMessage(int waitTimeInSeconds)
{
    int startTimer = waitTimeInSeconds;
    Console.WriteLine("");
    while (waitTimeInSeconds >= 0)
    {
        Console.SetCursorPosition(0, Console.CursorTop - 1);
        writeBeforeProgressBarLine("Too Many Requests! Waiting for " + waitTimeInSeconds + " seconds");
        Thread.Sleep(1000);
        waitTimeInSeconds--;
    }
    Console.SetCursorPosition(0, Console.CursorTop - 1);
    Console.Write("                                                     ");
    writeBeforeProgressBarLine("Waited " + startTimer + " seconds");
}