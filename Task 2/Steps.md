### Stored Procedures

Create test project for stored procedures.

Write tests.

Update dashboard stored procedures.



#### Clients by age

Min/max age become dates. Filter before adding bands. Discuss how bands are used (would this be better done in application, or is it required for reporting. Is a separate procedure for banded ages applicable).



#### Estimated net worth

As of parameter becomes date so no need to convert.



#### Stale clients

Compare as Dates.



#### Status breakdown

Remove order by. Use a join instead of subquery.

### 

### Application Rebuild



Discuss use of Entity Framework to increase development productivity / re-usability / testability.

Discuss when to use stored procedure over entity framework queries.



#### Backend

New ASP.Net project 

N-Tier code architecture (controller, service, repository).



New library for database, promoting reusability and separation of concerns. Store entities and migrations.



Create client read endpoint. Create service. Create repository.

Create simple flow to test with postman.

Compare with legacy results.



Write tests.



#### Frontend



Create React Project.

Connect to API.

Create clearly defined flow ( component -> service -> API ).

Retrieve and display data.

Compare to legacy results.





Complete application one feature at a time.



