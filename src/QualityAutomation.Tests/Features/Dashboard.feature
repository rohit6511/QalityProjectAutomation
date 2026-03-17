@Dashboard @Regression
Feature: Dashboard (Secure Area) Functionality
    As a logged-in user
    I want to access the secure area dashboard
    So that I can view secure content and logout

    # Requirements Covered:
    # REQ-005: User shall see secure area after successful login
    # REQ-006: User shall be able to logout from secure area
    # REQ-007: System shall display welcome message after login
    # REQ-008: User shall be able to re-login after logout

    Background:
        Given I am logged in with valid credentials

    @Smoke @Critical @REQ-005 @TC-DASH-001
    Scenario: Verify secure area page elements
        Then I should see the secure area header
        And I should see the logout button
        And the page URL should contain "secure"

    @Smoke @REQ-006 @TC-DASH-002
    Scenario: Successful logout from secure area
        When I click the logout button
        Then I should be redirected to the login page
        And I should see a logout success message

    @Regression @REQ-007 @TC-DASH-003
    Scenario: Verify welcome message on secure area
        Then I should see a success message containing "You logged into a secure area!"
        And the secure area header should be "Secure Area"

    @Regression @REQ-007 @TC-DASH-004
    Scenario: Verify secure area subheader text
        Then I should see the welcome subheader text
        And the subheader should contain "Welcome to the Secure Area"

    @Regression @REQ-008 @TC-DASH-005
    Scenario: Logout and login again
        When I click the logout button
        Then I should be redirected to the login page
        When I enter valid username "tomsmith"
        And I enter valid password "SuperSecretPassword!"
        And I click the login button
        Then I should be redirected to the secure area
