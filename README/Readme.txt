Key points:
-I moved the function which calculates distances between coordinates from C# to Linq to Entities, in this way the evalutation is DB-side.
-Repository Pattern, in this way I can change DB type (nosql) without rewrite a lot of code


Potential improvement (performance, scalability, diagnosis)

-MongoDb 
-Try to semplify the math function to calculate the distance (inserting minimal error, but improving speed)
-Precalculate some parameters and store them on DB (distancing in km between meridian in that geographic area)
-Cache on external server (Couchbase)
-Logging
-Asynch task
-Improving Unit tests


