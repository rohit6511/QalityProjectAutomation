@Login @Smoke
Feature: Login Functionality
    As a user
    I want to be able to login to the application
    So that I can access secure features 

    # Requirements Covered:
    # REQ-001: User shall be able to login with valid credentials
    # REQ-002: System shall reject invalid username
    # REQ-003: System shall reject invalid password
    # REQ-004: System shall display appropriate error messages for failed login

    @Negative @REQ-003 @REQ-004 @TC-LOGIN-003
    Scenario: Failed login with invalid password
        Given I am on the login page
        When I enter valid username "rohit.saini@thecodeinsight.com"
        And I enter invalid password "wrongpassword"
        And I click the login button
        Then I should remain on the login page
        And I should see an error message

    @Negative @REQ-002 @REQ-004 @TC-LOGIN-002
    Scenario: Failed login with invalid username
        Given I am on the login page
        When I enter invalid username "invaliduser@test.com"
        And I enter valid password "Rohit@6511"
        And I click the login button
        Then I should remain on the login page
        And I should see an error message
   
    @Positive @REQ-001 @TC-LOGIN-001
    Scenario Outline: Login with multiple valid user credentials
        Given I am on the login page
        When I enter valid username "<username>"
        And I enter valid password "<password>"
        And I click the login button
        Then I should be redirected to the secure area

        Examples:
            | username                       | password   |
            | rohit.saini@thecodeinsight.com | ROhit@6511 |
