document.getElementById("orderpricesum").innerHTML = $('#Quantity').val() * 10;

function parseDeltaCall(data) {

    // data is a plain javascript object
    //[object Object]
    const plain_object = { name: "value" }
    const plain_object1 = new Object()
    plain_object1.key = "value"
    

    // data is not plain js object
    const array = [1, 2, 3]
    const date = new Date()
    const err = new Error("message")
    const regex = /abc/;
    function myFunction() {
        this.name1 = "Hello, world!";
    }
    const fun = new myFunction();
    fun.name1;
    class Person {
        constructor(name, age) {
            this.name = name;
            this.age = age;
        }
        greet() {
            console.log("Hello, " + this.name);
        }
    }
    const person = new Person("alice", 20);
    person.greet();

    // string of data variable object type  
    Object.prototype.toString.call(data)
}