Feature: Article Getting by Short Url
	In order to ensure that search works like expected.

Scenario: Get article by key
	Given New York Times API returns two articles. First one has short url "https://nyti.ms/2YNxSD2" 
	And second one has short url "https://nyti.ms/2YNxSD3"
	When I get article by key "2YNxSD2"
	Then rest client will use "svc/topstories/v2/home.json" like resource 
	And the result will be an article with short url "https://nyti.ms/2YNxSD2"

