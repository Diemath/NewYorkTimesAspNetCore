Feature: Articles Getting API Integration
	In order to ensure correct using of New York Times API.
	In order to ensure using of filtration on New York Times API side to release backend from extra unnecessary work.

Background: 
	Given backend configuration file has such base API url "https://api.someapi.com/"
	And such API key "some-key" 
	And such resource "svc/topstories/v2/{section}.json"

Scenario: Get articles by some section
	When I get articles by some section
	Then backend rest client will use "https://api.someapi.com/" like base API url
	And "svc/topstories/v2/arts.json" like resource 
	And will has a parameter "api-key" with value "some-key"

Scenario: Get articles by some section and some updated date
	When I get articles by some section and some updated date
	Then backend rest client will use "https://api.someapi.com/" like base API url
	And "svc/topstories/v2/arts.json" like resource 
	And will has a parameter "api-key" with value "some-key"

Scenario: Get article groups by some section
	When I get article groups by some section
	Then backend rest client will use "https://api.someapi.com/" like base API url
	And "svc/topstories/v2/arts.json" like resource 
	And will has a parameter "api-key" with value "some-key"

Scenario: Get article by some key
	When I get article by some key
	Then backend rest client will use "https://api.someapi.com/" like base API url
	And "svc/topstories/v2/home.json" like resource 
	And will has a parameter "api-key" with value "some-key"
