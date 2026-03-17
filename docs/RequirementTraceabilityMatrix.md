# Requirement Traceability Matrix (RTM)

## Project: Quality Automation Framework
## Version: 1.0
## Last Updated: March 2026

---

## Overview

This document maps business requirements to test cases, ensuring complete test coverage and traceability throughout the testing lifecycle.

---

## Requirements Summary

| Req ID | Module | Requirement Description | Priority | Status |
|--------|--------|------------------------|----------|--------|
| REQ-001 | Login | User shall be able to login with valid credentials | High | Implemented |
| REQ-002 | Login | System shall reject invalid username | High | Implemented |
| REQ-003 | Login | System shall reject invalid password | High | Implemented |
| REQ-004 | Login | System shall display appropriate error messages for failed login | Medium | Implemented |
| REQ-005 | Dashboard | User shall see secure area after successful login | High | Implemented |
| REQ-006 | Dashboard | User shall be able to logout from secure area | High | Implemented |
| REQ-007 | Dashboard | System shall display welcome message after login | Medium | Implemented |
| REQ-008 | Dashboard | User shall be able to re-login after logout | Medium | Implemented |

---

## Traceability Matrix

| Req ID | Test Case ID | Test Scenario | Feature File | Tags | Status |
|--------|--------------|---------------|--------------|------|--------|
| REQ-001 | TC-LOGIN-001 | Login with multiple valid user credentials | Login.feature | @Positive @REQ-001 | Active |
| REQ-002 | TC-LOGIN-002 | Failed login with invalid username | Login.feature | @Negative @REQ-002 | Active |
| REQ-003 | TC-LOGIN-003 | Failed login with invalid password | Login.feature | @Negative @REQ-003 | Active |
| REQ-004 | TC-LOGIN-002, TC-LOGIN-003 | Error message validation | Login.feature | @Negative @REQ-004 | Active |
| REQ-005 | TC-DASH-001 | Verify secure area page elements | Dashboard.feature | @Smoke @Critical @REQ-005 | Active |
| REQ-006 | TC-DASH-002 | Successful logout from secure area | Dashboard.feature | @Smoke @REQ-006 | Active |
| REQ-007 | TC-DASH-003, TC-DASH-004 | Verify welcome message and subheader | Dashboard.feature | @Regression @REQ-007 | Active |
| REQ-008 | TC-DASH-005 | Logout and login again | Dashboard.feature | @Regression @REQ-008 | Active |

---

## Test Case Details

### Login Module

#### TC-LOGIN-001: Login with Valid Credentials
- **Requirement**: REQ-001
- **Priority**: High
- **Type**: Positive
- **Steps**:
  1. Navigate to login page
  2. Enter valid username
  3. Enter valid password
  4. Click login button
- **Expected Result**: User is redirected to secure area

#### TC-LOGIN-002: Failed Login with Invalid Username
- **Requirement**: REQ-002, REQ-004
- **Priority**: High
- **Type**: Negative
- **Steps**:
  1. Navigate to login page
  2. Enter invalid username
  3. Enter valid password
  4. Click login button
- **Expected Result**: User remains on login page with error message

#### TC-LOGIN-003: Failed Login with Invalid Password
- **Requirement**: REQ-003, REQ-004
- **Priority**: High
- **Type**: Negative
- **Steps**:
  1. Navigate to login page
  2. Enter valid username
  3. Enter invalid password
  4. Click login button
- **Expected Result**: User remains on login page with error message

### Dashboard Module

#### TC-DASH-001: Verify Secure Area Page Elements
- **Requirement**: REQ-005
- **Priority**: High
- **Type**: Smoke
- **Precondition**: User is logged in
- **Steps**:
  1. Verify secure area header is displayed
  2. Verify logout button is visible
  3. Verify URL contains "secure"
- **Expected Result**: All elements are present

#### TC-DASH-002: Successful Logout
- **Requirement**: REQ-006
- **Priority**: High
- **Type**: Smoke
- **Precondition**: User is logged in
- **Steps**:
  1. Click logout button
  2. Verify redirect to login page
  3. Verify logout success message
- **Expected Result**: User is logged out successfully

#### TC-DASH-003: Verify Welcome Message
- **Requirement**: REQ-007
- **Priority**: Medium
- **Type**: Regression
- **Precondition**: User is logged in
- **Steps**:
  1. Verify success message contains expected text
  2. Verify secure area header
- **Expected Result**: Welcome message is displayed correctly

#### TC-DASH-004: Verify Subheader Text
- **Requirement**: REQ-007
- **Priority**: Medium
- **Type**: Regression
- **Precondition**: User is logged in
- **Steps**:
  1. Verify subheader is displayed
  2. Verify subheader contains expected text
- **Expected Result**: Subheader displays correct content

#### TC-DASH-005: Logout and Re-login
- **Requirement**: REQ-008
- **Priority**: Medium
- **Type**: Regression
- **Precondition**: User is logged in
- **Steps**:
  1. Click logout button
  2. Verify redirect to login page
  3. Enter valid credentials
  4. Click login button
  5. Verify redirect to secure area
- **Expected Result**: User can successfully re-login

---

## Coverage Summary

| Module | Total Requirements | Tests Mapped | Coverage % |
|--------|-------------------|--------------|------------|
| Login | 4 | 4 | 100% |
| Dashboard | 4 | 4 | 100% |
| **Total** | **8** | **8** | **100%** |

---

## Test Execution History

| Date | Environment | Total Tests | Passed | Failed | Skipped | Coverage |
|------|-------------|-------------|--------|--------|---------|----------|
| - | - | - | - | - | - | - |

*Note: This section will be updated after each test execution*

---

## How to Run Tests by Requirement

```bash
# Run all tests for a specific requirement
dotnet test --filter "Category=REQ-001"

# Run all Login requirement tests
dotnet test --filter "Category=REQ-001|Category=REQ-002|Category=REQ-003|Category=REQ-004"

# Run all Dashboard requirement tests
dotnet test --filter "Category=REQ-005|Category=REQ-006|Category=REQ-007|Category=REQ-008"
```

---

## Approval

| Role | Name | Signature | Date |
|------|------|-----------|------|
| QA Lead | | | |
| Dev Lead | | | |
| Project Manager | | | |

---

## Revision History

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | March 2026 | QA Team | Initial RTM creation |
