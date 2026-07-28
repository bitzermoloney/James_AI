# James_AI
An AI Chatbot

# Context
In school, I was tasked to create a chatbot as park of my GSCE in ICT, which I did in python. It used basic if loops and replied with a set of random responses which I had programmed in, a little like one of the world's first and most basic AI concept chatbots, ELIZA. Recently, a friend from college who is studying CS at Newcastle University mentioned that he had created an AI chatbot, which worked more like the modern Claude and GPT models by generating it's own response. AI is something which interests me because I wonder about the future of the professional world and how it will develop to work onlongside humans. Therefore, I decided to create this prototype AI chatbot using C#.

# Decomposing the task
I needed to break the task down into steps:
- Download a software dev kit (SDK). I have two options here; download the OpenAI SDK, making my life very easy, or use plain HTTP requests, which would be the hard way to do this. I chose the hard option (HTTP requests). After all, the whole point of expanding my portfolio, so I might as well learn as much as possible while creating this AI chatbot.
- Store my API, where the program will run / report to.
- Create the chatbot class.
- Connect it to the API.
- Put the class in a loop (so it runs for ever).
- Add memory, so conversations are seemless.
- Improve the UI (as much as possible considering this is a console app).
I also created some tasks I could maybe do to stretch myself:
- Add saving memory (file handling).
- Give the chatbot tools such as the internet or weather.
This is a project that can never be complete, there are many things I could add after this.

# Getting started
- I set up the workspace with HTTP requests to an OpenAI-compatible API, so I can possibly switch SDK further down the line if I feel like it.
- Added a simple chat loop.
- Added environment based configs for the API key and model.
- The program runs by exporting the API key, running the program and if you want to quit, type 'exit'.
