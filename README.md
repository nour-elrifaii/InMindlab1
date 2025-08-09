# Hi Ramy!!
This is my submission of Lab4.

## Exception Handling
Once I inserted the validator some of the exception handling was useless to keep so i removed them from my endpoints.

## Middleware
The information from the HTTP context is taken and displayed on the command line. I displayed the timestamp as only the time the request was made on and not as a stopwatch to show the elapsed time of the request.

## Validation
The validator makes sure that the incoming data is valid before it is being processed. I had to remove the try catch statemnets from my endpoints cause it felt like I was repeating myself.

## Filters
This filter finds and runs the registered IValidor. If it contains errors, it returns a message with the error details.

## Reflection & Generic Type
The Person class was created to create a generic mapper with the student class. ObjectMapperService in Services contains a simple mapper. It uses reflection to create a new instance of a target type and copy over values for properties that share the same names. This is tested in the "mapStudentToPerson" endpoint, where the request takes a stduent and send sback a person.
