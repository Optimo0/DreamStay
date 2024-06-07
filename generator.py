from random import choice
import pandas as pd
from faker import Faker

adjectives = ["Beautiful", "Fancy", "Happy", "Imperial",
              "Luxury", "Quiet", "Rosy", "Spring", "Sunny", 
              "Urban", "Dreamy", "Gleamy", "Peaceful"]
nouns = ["Beach", "Downs", "Gardens", "Heights", "Hills",
         "Lake", "Meadows", "Mountains", "Sea", "Shore", "Valley", 
         "Cottage", "Abode"]
suffixes = ["Apartments", "B&B", "Club", "Hotel",
            "Inn", "Resorts", "Spa", "Suites", "Towers"]


def get_hotel_names(count=1, adjective=None, noun=None, suffix=None):
    names = []
    for _ in range(count):
        adjective_ = adjective or choice(adjectives)
        noun_ = noun or choice(nouns)
        suffix_ = suffix or choice(suffixes)
        names.append(f"{adjective_} {noun_} {suffix_}")
    return names
    
    
def save_to_file(filename, count):
    with open(filename, 'w') as file:
        for name in get_hotel_names(count):
            file.write(f'{name}\n')


def get_fake_country(count=1):
    fake = Faker()
    return [fake.country() for _ in range(count)]


def get_fake_city(count=1):
    fake = Faker()
    return [fake.city() for _ in range(count)]


def main():
    save_to_file('hotel_names.txt', 1000)
    print(get_fake_city(10))

if __name__ == '__main__':
    main()
