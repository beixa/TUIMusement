
# TUIMusement # | Backend tech homework

**Full Name**: Iker Beitia Vaz

### Step 1 | Development
To run this locally we just need Docker Desktop installed.
Once we've got the code cloned or downloaded, from the console and once we are in the TUIMusement folder:
```sh
$ cd /TUIMusement
$ dotnet publish -c Release
$ docker build -t tuimusement-image -f Dockerfile .
$ docker run -it --rm tuimusement-image
```


### Step 2  | API design

#### · Endpoint to set the forecast for a specific city
`PUT /api/v3/cities/{id}/forecast`

##### Parameters:
* `id`: City identifier

##### Payload:
The payload would be a list of elements containing the date and condition to be updated or created.
```json
[
  {"date":"2022-08-03","condition":"Sunny"},
  {"date":"2022-08-04","condition":"Sunny"}
]
```

##### Possible responses:
* **200:** Returned when successful.
* **404:** Resource not found.
* **503:** Service unavailable.


#### · Endpoint to get the forecast for a specific city
`GET /api/v3/cities/{id}/forecast?days={days}`

##### Parameters:
* `id`: City identifier
* `days`: Number of days of forecast required. days parameter value ranges between 1 and 14. If no days 			parameter is provided then only today's weather is returned.

##### Payload:
None

##### Possible responses:
* **200:** Returns a list of days requested. Example:
 ```json
[
  {"date":"2022-08-03","condition":"Sunny"},
  {"date":"2022-08-04","condition":"Sunny"}
]
```
* **400:** When days parameter range is not between 1 and 14.
* **404:** Resource not found.
* **503:** Service unavailable.