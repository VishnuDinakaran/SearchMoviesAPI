# SearchMoviesAPI
Search engine API

Available methods on Movies API 
 
(API A) - SearchMovies
 Eg 1             curl -H "Content-Type: application/json" -X GET -d "{ 'RequestId':'278C48BE-27E4-497B-BFD1-80404FD67C97', 'Query':{ 'PropertyName':'Title', 'operator':'==', 'value':'Rio', 'Queries': [{'PropertyName':'YearOfRelease', 'operator':'==', 'value':'2011', 'Junction':'And', 'Index':'2'}, {'PropertyName':'Genres', 'operator':'!=', 'value':'Action', 'Junction':'And', 'Index':'1'}]}}" https://localhost:44371/api/Movies/SearchMovies -i
 Eg 2             curl -H "Content-Type: application/json" -X GET -d "{ 'RequestId': '278C48BE-27E4-497B-BFD1-80404FD67C97', 'Query': { 'PropertyName': 'Genres', 'operator': 'stringcontains', 'value': 'Action', 'Queries': [ { 'PropertyName': 'YearOfRelease', 'operator': '>', 'value': '1996', 'Junction': 'AND' } ] } }" https://localhost:44371/api/Movies/SearchMovies -i
 
(API B) - SearchTop5MoviesByUserAverageRating
                curl -H "Content-Type: application/json" -X GET -d "{ 'RequestId': '278C48BE-27E4-497B-BFD1-80404FD67C97', 'Query': { 'PropertyName': 'Genres', 'operator': 'stringcontains', 'value': 'Action', 'Queries': [ { 'PropertyName': 'YearOfRelease', 'operator': '>', 'value': '1993', 'Junction': 'AND' } ] } }" https://localhost:44371/api/Movies/SearchTop5MoviesByUserAverageRating -i
 
(API C) - SearchTop5MoviesByOneUserRating
                curl - H "Content-Type: application/json" - X GET - d "{ 'RequestId': '278C48BE-27E4-497B-BFD1-80404FD67C97', 'Query': { 'PropertyName': 'UserName', 'operator': '==', 'value': 'Tom' } }" https://localhost:44371/api/Movies/SearchTop5MoviesByOneUserRating -i
 
(API D) - Update User Ratting
                curl -H "Content-Type: application/json" -X PUT -d "{ 'Id': '278C48BE-27E4-497B-BFD1-80404FD67C97', 'UserRating': { 'MovieId': '4', 'MovieTitle':'The Mummy' , 'UserName':'John', 'UserRatingValue': 2.2} }" https://localhost:44371/api/Movies/UpdateUserRating -i
