Feature: HotelBooking

Scenario: start date after, and > end date, end date after
	Given start date after, and > end date
	And end date after occupied time
	When creating a booking
	Then the booking should not be created
	
Scenario: Start date before, end date before
	Given start date before occupied time 2
	And end date before occupied time 2
	When creating a booking 2
	Then the booking should be created 2

Scenario: Start date after, end date after
	Given start date after occupied time 3
	And end date after occupied time 3
	When creating a booking 3
	Then the booking should be created 3

Scenario: Start date before, end date after
	Given start date before occupied time 4
	And end date after occupied time 4
	When creating a booking 4
	Then the booking should not be created 4

Scenario: Start date in occupied, end date after occupied
	Given start date in occupied time 5
	And end date after occupied time 5
	When creating a booking 5
	Then the booking should not be created 5

Scenario: start date before, end date in occupied
	Given start date before occupied time 6
	And end date in occupied time 6
	When creating a booking 6
	Then the booking should not be created 6
