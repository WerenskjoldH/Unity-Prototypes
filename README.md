<p align="center"><img width=100% src="media/unityPrototypesTitle.png"></p>

<h4 align="center">A Place Where Prototypes Proliferate</p>

[![Repository Size](https://img.shields.io/github/repo-size/WerenskjoldH/Unity-Prototypes)]()

---

---

## Purpose

This repository is simply what the title suggest it is. I use this repo to store various Unity-based prototypes I am working on. 

---

## What ARE you working on?:  

### The Tower

### Swap Shot

### Wall Jump

## Dependencies

This project was built for the [SFML 2.5 API](https://www.sfml-dev.org/), and as such, you will need to link the library to your project to use this input manager.

---

## Q&A

### _Q_: What if I don't want to use an integer to reference keys?

**A**: Don't worry, the adjustment is quite simple! You will just need to modify the mapping from `int -> Key` to `std::string -> Key` and make adjustments to all references to the map to pass a string instead of int. Huzzah!