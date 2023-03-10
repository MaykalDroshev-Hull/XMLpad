// Define a function that adds two numbers
function addNumbers(a, b) {
  return a + b;
}

// Define an object that represents a person
var person = {
  firstName: "John",
  lastName: "Doe",
  age: 30,
  hobbies: ["reading", "running", "swimming"],
  address: {
    street: "123 Main St",
    city: "Anytown",
    state: "CA",
    zip: "12345"
  },
  getFullName: function() {
    return this.firstName + " " + this.lastName;
  }
};

// Call the addNumbers function and display the result in the console
var result = addNumbers(5, 10);
console.log("The result of adding 5 and 10 is: " + result);

// Access and display properties of the person object in the console
console.log("Full name: " + person.getFullName());
console.log("Age: " + person.age);
console.log("Hobbies: " + person.hobbies.join(", "));
console.log("Address: " + person.address.street + ", " + person.address.city + ", " + person.address.state + " " + person.address.zip);
