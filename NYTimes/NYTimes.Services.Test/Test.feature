Feature: Articles Getting
	In order to ensure correct using of New York Times API

Background: 
	Given backend configuration file has such base API url - "https://api.someapi.com/", such API key - "some-key" and such resource "svc/topstories/v2/{section}.json"

Scenario: Get articles by section
	When I get articles by section "Arts"
	Then backend rest client will use "https://api.someapi.com/" like base API url, "svc/topstories/v2/arts.json" like resource and will has a parameter "api-key" with value "some-key"

Scenario: Get articles by section and date
	When I get articles by section "Arts" and date "6/10/2019"
	Then backend rest client will use "https://api.someapi.com/" like base API url, "svc/topstories/v2/arts.json" like resource and will has a parameter "api-key" with value "some-key"