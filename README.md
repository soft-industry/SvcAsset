# SvcAsset

Some comments about the test task

1) During the process of testing, I have faced many exceptions "The entity type requires a primary key to be defined", so, I have decided to inherit the entities without primary keys from BaseEntity class.
2) The type for PurposeId used in the project remains Purpose enumeration.
3) Per my understanding about task 3: if another event is created for the same asset but with start and\or end dates which intersect already created ones - error message is sent, otherwise - the new event is created successfully.
4) I'm not clear about the user input for EventTime. At the current moment, it is created from the ReservationModel.EventTime which is populated when the request (CreateEventRequest) is created. It looks like there should be functionality for users to create EventTime itself. Alternatively required fields could be added as other properties for the same request.
5) I'm not clear what should be done with the following Event properties EventLock, LU_Date, LU_User. Looks like exactly in those properties should be stored some kind of user's data, but I'm not sure.'
6) I haven't removed old properties EventStart, EventEnd from Event, and ReservationModel classes because they are used by some Queries.