@Login @Smoke
Feature: Login Functionality
    As a user
    I want to be able to login to the application
    So that I can access secure features 

    # @Negative
    # Scenario: Failed login with invalid username
    #     Given I am on the login page
    #     When I enter invalid username "invaliduser@test.com"
    #     And I enter valid password "Rohit@6511"
    #     And I click the login button
    #     Then I should remain on the login page
    #     And I should see an error message

    @Negative
    Scenario: Failed login with invalid password
        Given I am on the login page
        When I enter valid username "rohit.saini@thecodeinsight.com"
        And I enter invalid password "wrongpassword"
        And I click the login button
        Then I should remain on the login page
        And I should see an error message

   
    @Positive
    Scenario Outline: Login with multiple valid user credentials
    Given I am on the login page
        When I enter valid username "<username>"
        And I enter valid password "<password>"
        And I click the login button
        Then I should be redirected to the secure area

        Examples:
            | username                       | password   |
            | rohit.saini@thecodeinsight.com | ROhit@6511 |
