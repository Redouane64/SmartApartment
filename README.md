## Smart Apartement Data 

### Search

Query example:

- With `limit` parameter:

```
http://localhost:5000/search?keyword=oak%20hill&limit=10
```

- With `markets` parameter (comma separated values):
```
http://localhost:5000/search?keyword=oak%20hill&markets=atlanta,sacramento
```

### example output:

```

{
    "size": 25,
    "items": [
        {
            "property": {
                "propertyId": 78925,
                "name": "Oak Hill",
                "market": "Atlanta",
                "state": "GA",
                "formerName": "",
                "streetAddress": "105 Oak Hill Drive",
                "city": "Athens",
                "latitude": 33.97513,
                "longitude": -83.35711
            }
        },
        {
            "property": {
                "propertyId": 73933,
                "name": "Club at Stonegate",
                "market": "DFW",
                "state": "TX",
                "formerName": "Oak Hill",
                "streetAddress": "2450 Oak Hill Cir",
                "city": "Fort Worth",
                "latitude": 32.71502137184143,
                "longitude": -97.38161623477936
            }
        },
        {
            "property": {
                "propertyId": 74621,
                "name": "Dixon at Stonegate",
                "market": "DFW",
                "state": "TX",
                "formerName": "Villas of Oak Hill",
                "streetAddress": "2501 Oak Hill Cir",
                "city": "Fort Worth",
                "latitude": 32.716346,
                "longitude": -97.379417
            }
        }

        ....
    }
}


```