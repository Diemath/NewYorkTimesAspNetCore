Feature: Articles Getting API Integration
	In order to ensure correct using of New York Times API.
	In order to ensure using of filtration on New York Times API side to release backend from extra unnecessary work.

Background: 
	Given backend configuration file has such base API url "https://api.someapi.com/"
	And such API key "some-key" 
	And such resource "svc/topstories/v2/{section}.json"

Scenario: Get articles by section
	When I get articles by section "Arts"
	Then backend rest client will use "https://api.someapi.com/" like base API url
	And "svc/topstories/v2/arts.json" like resource 
	And will has a parameter "api-key" with value "some-key"

Scenario: Get articles by section and updated date
	When I get articles by section "Arts" and updated date "6/10/2019"
	Then backend rest client will use "https://api.someapi.com/" like base API url
	And "svc/topstories/v2/arts.json" like resource 
	And will has a parameter "api-key" with value "some-key"

Scenario: Get article groups by section
	When I get article groups by section "Arts"
	Then backend rest client will use "https://api.someapi.com/" like base API url
	And "svc/topstories/v2/arts.json" like resource 
	And will has a parameter "api-key" with value "some-key"
