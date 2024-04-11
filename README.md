# Recipes for College Students on a Budget
## Persona
Our persona is Chad, a 19 year old college student who cant write English essays. He has limited kitchen appliances and absolutely no time or skills devoted to cooking. He is also broke and has 3 roommates who have basic cooking skills. However, with their schedules, they do not have time to cook any meals and have trouble finding recipes that can be done in a limited amount of time. This website will be targeted to people like Chad and his unfortunate roommates. So, on our website, we will dedicate a tab to quick recipes under 30 minutes, and each recipe will also include a difficulty rating. Our application will have a very simple but nice layout that is easy on the eyes and not too overbearing in terms of information. We will highlight what is most important to college students, and they will also have quick access to searching recipes with certain tags (to sort out ingredients). With the difficulty rating (which will be a culmination of ratings from other students like Chad), they will be able to decide whether a recipe is fit for them or not. Lastly, the quick tab to short recipes under 30 minutes will also help out these students, who are too busy with their studies to worry about overnight marination or 5-hour crockpots!

## Agreements
What will you do to ensure your code is readable? For example, will you share your code frequently
with your partner? Setup Visual Studio Code’s auto formatter consistently across group members
We will add comments and documentation on our code. Descriptive variable names and implement helper methods to modularize and make our code readable.

What procedure will you put in place to ensure committed code is functional?
We will test our code before commiting and merging. We will also use feature branches, and when we know the code is functional, we will submit a merge request and have our partners verify the logistics of the code and approve the request.

How do you plan to test your code?
We will use the debugger, and and write unit tests as we write our methods. each method will thus include multiple tests for any cases we can think of, and we will run these tests.

How do you plan to divide the work?
We will split the features by weight evenly, and also assign the work based on someone's strengths and what they feel more comfortable writing and working on.

How will you ensure that your application is robust and does not fail due to user errors?
We will make sure unit tests cover all edge cases, and once the application is done, we will run and test it thouroughly. We will also make sure to handle any exceptions that may occur in the c# application layer.

How will you ensure your stand alone classes can be tested?
We will make sure each class has a specific functionality and serves a purpose, and that they have methods that perform a specific, useful task that return values or manipulate the object.

## Extra Features:
1. including a difficulty rating for each of the recipes (as a rating system that the people who try the recipe can set)
2. We will implement the budget system
4. unit conversions between units for Mass and Volume.

## Design Choices
For this project, we have the basic objects we need. Here is a brief overview of how we will handle each category:

For the recipes, we have our basic recipe object which contains all functionalities for a single recipe. We then have a class Recipes, and this will represent the actions such as deleting a recipe and creating a recipe, in which we need to modify the list of recipes and not just a single one. This is where we include our functionalities for filtering as well.

In line with recipes, we have an Ingredient struct which will represent a single ingredient and contain its cost per unit as well as unit of measurement. The Measurement class will take care of converting the measurements from one unit to another (provided they are both of the same Units category). The logic behind this is to choose a common base unit to convert to and from!

For users, we have our basic user class which contains all the actions involving a single user. We then have a Users class, which contains an active user (the currently logged in user) and the actions that will affect the list of users in the database (authenticate user, in which you need to sift through all the usernames and then match the password, and deleting an account, in which the user will remove themselves from the list.) Linked to this we have a Password class which will manage hashing the password and validating whether two passwords match.

Lastly, we have our IFilterBy, an interface for filtering our recipes. This interface will be implemented for each different sort we will need for this application. An IFilterBy List field will be contained in the Recipes class to keep track of how the user will want to filter their recipes (provided they give several filters).
