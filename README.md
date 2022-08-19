# BeautySalon
 It is a Console application that allows you to operate a hairdressing salon.
The individual functionalities are available only to the appropriate users. For example, a customer cannot remove an employee, only an employee who is the boss can do so. 
The application enables:
- adding customers, employees and bosses;
- remove of employees;
- logging in, changing the user and logging out;
- creating weekly schedules by employees;
- customers sign up for services when term is free;
- displaying employees, services and schedules according to selected conditions.
With the first launch of the program (when a given table in the database is empty), employees and boss will be saved to the database from the txt files 
in the bin / Debug / net6.0 / Entitiess directory. Also with the first launch, a client, a schedule for an employee with index 1 and a list of services are added.
All users entered at the first launch of the program have the password "lol".
When using the program, enter the data appropriately using the hints, for example:
when the program asks you to enter the time, in brackets it will be (HH: mm), so enter for example "08:00" and press enter.
In the bin / Debug / net6.0 / Entitiess directory, there is the audit.txt file that records the operations performed in the application.
