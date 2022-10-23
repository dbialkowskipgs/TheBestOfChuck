# TheBestOfChuck

## Before start function:
Add your X-RapidAPI-Key in launchSettings.json file under ChuckJokesTrigger project properties

{
  "profiles": {
    "ChuckJokesTrigger": {
      "commandName": "Project",
      "commandLineArgs": "--port 7238",
      "launchBrowser": false,
      "environmentVariables": {
        "JokesAmount": "<NUMBER_OF_PULLING_JOKES",
        "DbName": "<DB_NAME>",
        "ConnectionString": "<DB_CONNECTION STRING>",
        "Api_Endpoint": "<API_ENDPOINT>",
        "X-RapidAPI-Key": "<YOUR_PRIVATE_API_KEY>",
        "X-RapidAPI-Host": "<API_HOST>",
        "accept": "application/json"
      }
    }
  }
}