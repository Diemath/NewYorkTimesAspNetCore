Feature: Use correct request url

Scenario: Use correct request url taken from official documentation
	Given injected configuration has such base API url - "https://api.someapi.com/", such API identifier - "xxxxxxxxx" and such resource "svc/topstories/v2/{section}.json"
	When I get articles by section "Arts"
	Then the rest client will use "https://api.someapi.com/" like base API url, "svc/topstories/v2/arts.json" like resource and will have a parameter "api-key" with value "xxxxxxxxx"