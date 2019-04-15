# SearchMoviesAPI
Search engine API

Available methods on Movies API 
 
(API A) - SearchMovies <BR>
 Eg 1  <BR>           curl -H "Content-Type: application/json" -X GET -d "{ 'RequestId':'278C48BE-27E4-497B-BFD1-80404FD67C97', 'Query':{ 'PropertyName':'Title', 'operator':'==', 'value':'Rio', 'Queries': [{'PropertyName':'YearOfRelease', 'operator':'==', 'value':'2011', 'Junction':'And', 'Index':'2'}, {'PropertyName':'Genres', 'operator':'!=', 'value':'Action', 'Junction':'And', 'Index':'1'}]}}" https://localhost:44371/api/Movies/SearchMovies -i
 <BR>Eg 2  <BR>           curl -H "Content-Type: application/json" -X GET -d "{ 'RequestId': '278C48BE-27E4-497B-BFD1-80404FD67C97', 'Query': { 'PropertyName': 'Genres', 'operator': 'stringcontains', 'value': 'Action', 'Queries': [ { 'PropertyName': 'YearOfRelease', 'operator': '>', 'value': '1996', 'Junction': 'AND' } ] } }" https://localhost:44371/api/Movies/SearchMovies -i
 <BR>
(API B) - SearchTop5MoviesByUserAverageRating <BR>
  <BR>              curl -H "Content-Type: application/json" -X GET -d "{ 'RequestId': '278C48BE-27E4-497B-BFD1-80404FD67C97', 'Query': { 'PropertyName': 'Genres', 'operator': 'stringcontains', 'value': 'Action', 'Queries': [ { 'PropertyName': 'YearOfRelease', 'operator': '>', 'value': '1993', 'Junction': 'AND' } ] } }" https://localhost:44371/api/Movies/SearchTop5MoviesByUserAverageRating -i
 <BR>
(API C) - SearchTop5MoviesByOneUserRating <BR>
 <BR>               curl - H "Content-Type: application/json" - X GET - d "{ 'RequestId': '278C48BE-27E4-497B-BFD1-80404FD67C97', 'Query': { 'PropertyName': 'UserName', 'operator': '==', 'value': 'Tom' } }" https://localhost:44371/api/Movies/SearchTop5MoviesByOneUserRating -i
 <BR>
(API D) - Update User Ratting <BR>
 <BR>               curl -H "Content-Type: application/json" -X PUT -d "{ 'Id': '278C48BE-27E4-497B-BFD1-80404FD67C97', 'UserRating': { 'MovieId': '4', 'MovieTitle':'The Mummy' , 'UserName':'John', 'UserRatingValue': 2.2} }" https://localhost:44371/api/Movies/UpdateUserRating -i
