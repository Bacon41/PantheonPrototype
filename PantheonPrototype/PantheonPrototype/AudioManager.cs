﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using FuncWorks.XNA.XTiled;

namespace PantheonPrototype
{
    public class AudioManager
    {
        public AudioEngine engine;
        public SoundBank sounds;
        public WaveBank waves;

        public AudioCategory backgroundMusicCategory;
        public AudioCategory soundEffectCategory;
        public AudioCategory gunshotCategory;

        float backgroundVolume = 1.0f;
        float sfxVolume = 1.0f;
        float gunshotVolume = 0.2f;

        Cue backgroundMusicCue = null;
        public Cue sfxCue;
        Cue previousMusic = null;


        public AudioManager()
        {
            engine = new AudioEngine("Content/Sound/XACT Sound File.xgs");
            waves = new WaveBank(engine, "Content/Sound/Wave Bank.xwb");
            sounds = new SoundBank(engine, "Content/Sound/Sound Bank.xsb");

            soundEffectCategory = engine.GetCategory("Sound Effect");
            soundEffectCategory.SetVolume(sfxVolume);

            backgroundMusicCategory = engine.GetCategory("Music");
            backgroundMusicCategory.SetVolume(backgroundVolume);

            //the default gun sound was much too loud, so I created its own category to set the 
            //volume for only that sound
            gunshotCategory = engine.GetCategory("Gunshot");
            gunshotCategory.SetVolume(gunshotVolume);

            Cue startSplashMusic = sounds.GetCue("Drum n Bass D Coexistant"); 
            playFirstBackgroundMusic(startSplashMusic);
        }

        /// <summary>
        /// This method will play the first set of music for the game
        /// </summary>
        /// <param name="backgroundCue">created cue that is attributed to specific sound</param>
        public void playFirstBackgroundMusic(Cue backgroundCue)
        {
            if (backgroundMusicCue == null)
            {
                backgroundMusicCue = backgroundCue;
                backgroundMusicCue.Play();
            }
        }

        /// <summary>
        /// This will play sound effects.. YAY!!!
        /// (based on the string cueName)
        /// </summary>
        /// <param name="cueName">the name of the cue that the XACT file attributes to the sound file</param>
        public void playSoundEffect(string cueName)
        {
            Cue soundEffect = sounds.GetCue(cueName);
            soundEffect.Play();

        }
        /// <summary>
        /// This will basically stop any background music that is playing, then will select the new 
        /// sound file based on the name of the current level 
        /// </summary>
        /// <param name="currentLevel">passed from Level (levelNum)</param>
        public void playBackgroundMusic(string currentLevel)
        {
            Cue newMusic;
            if (backgroundMusicCue.IsPlaying)
            {
                backgroundMusicCue.Pause();

            }
            if (previousMusic != null && previousMusic.IsPlaying)
            {
                previousMusic.Pause();
            }
            if (currentLevel.Equals("Maps/RebelFoyer"))
            {
                newMusic = sounds.GetCue("Fantastic A");
                newMusic.Play();
                previousMusic = newMusic;
            }
            else if (currentLevel.Equals("Maps/RebelMain"))
            {
                newMusic = sounds.GetCue("Electro DnB B Chaos");
                newMusic.Play();
                previousMusic = newMusic;
            }
            else if (currentLevel.Equals("Maps/map1"))
            {
                newMusic = sounds.GetCue("Chillectro");
                newMusic.Play();
                previousMusic = newMusic;
            }
            else if (currentLevel.Equals("Maps/RebelRoom1"))
            {
                newMusic = sounds.GetCue("Chill Deep");
                newMusic.Play();
                previousMusic = newMusic;
            }
            else if (currentLevel.Equals("Maps/RebelRoom2"))
            {
                newMusic = sounds.GetCue("Chill Dark");
                newMusic.Play();
                previousMusic = newMusic;
            }
            else if (currentLevel.Equals("Maps/RebelRoom3"))
            {
                newMusic = sounds.GetCue("Dark Rock");
                newMusic.Play();
                previousMusic = newMusic;
            }
            else if (currentLevel.Equals("Maps/RebelRoom4"))
            {
                newMusic = sounds.GetCue("Cool Chill Night");
                newMusic.Play();
                previousMusic = newMusic;
            }   

        }
    }
}

