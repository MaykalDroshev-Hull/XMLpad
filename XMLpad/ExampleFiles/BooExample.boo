class Animal:
    def MakeSound():
        raise NotImplementedException()

class Dog(Animal):
    def MakeSound():
        print "Woof!"

class Cat(Animal):
    def MakeSound():
        print "Meow!"

interface IPlayable:
    def Play()

class Toy:
    def Play():
        print "The toy is playing."

class ToyDog(Dog, IPlayable):
    def Play():
        print "The toy dog is playing."

class ToyCat(Cat, IPlayable):
    def Play():
        print "The toy cat is playing."

def Main():
    dog = Dog()
    dog.MakeSound()

    cat = Cat()
    cat.MakeSound()

    toydog = ToyDog()
    toydog.MakeSound()
    toydog.Play()

    toycat = ToyCat()
    toycat.MakeSound()
    toycat.Play()
