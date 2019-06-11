Feature: Articles Filtration
	In order to ensure that filtration works like expected.

Scenario: Filter articles by updated date 
	Given New York Times API returns two articles. First one has updated date "6/10/2019" 
	And second one has updated date "7/10/2019"
	When I filter articles by updated date "6/10/2019"
	Then the result will be "1" article 
	And its updated date will be "6/10/2019"

