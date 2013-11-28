using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cold_Ship
{
    public static class StringDialogue
    {
        /*************************************************  HOLDING CELL ************************************************************************/
        // Introduction Speech                                                                                                                  //
        public static String holdingCellIntroduction1 = "Good, you're awake.";                                                                  //
        public static String holdingCellIntroduction2 = "There isn't much time.";                                                               //
        public static String holdingCellIntroduction3 = "The ship is going down.";                                                              //
        public static String holdingCellIntroduction4 = "You need to fix it up if you want to live.";                                           //
        public static String holdingCellIntroduction5 = "You do want to live, don't you?";                                                      //
        public static String holdingCellIntroduction6 = "I hear space death isn't very pleasant, though.";                                      //
        public static String holdingCellIntroduction7 = "Up to you.";                                                                           //
        // Leaving the room without the lighter                                                                                                 //
        public static String holdingCellLeaveWithoutLighter = "You definitely should pick that lighter up before you get out of here.";         //
        /****************************************************************************************************************************************/

        /*************************************************  PRISON BLOCKS ***********************************************************************/
        // Starting speech                                                                                                                      //
        public static String prisonBlockStartingSpeech1 = "Looks like you're going to try to live.";                                            //
        public static String prisonBlockStartingSpeech2 = "In that case, you can start by turning the emergency lights back on.";               //
        // Speech before turning on the generator                                                                                               //
        public static String prisonBlockGeneratorSpeech1 = "You'll need to turn the power on in this room to get the door open.";               //
        public static String prisonBlockGeneratorSpeech2 = "There should be a switch for the lights too.";                                      //
        // Speech before leaving the room                                                                                                       //
        public static String prisonBlockLeavingRoom1 = "That's all there is to it.";                                                            //
        public static String prisonBlockLeavingRoom2 = "See if you can do the same in the other rooms.";                                        //
        public static String prisonBlockLeavingRoom3 = "You'll need to manually reactivate the emergency power in every section"                //
                                                     + "if you're going to reach the control room.";                                            //
        public static String prisonBlockLeavingRoom4 = "The main power can be turned back on from there.";                                      //
        /****************************************************************************************************************************************/

        /*************************************************  GENERATOR ROOM **********************************************************************/
        // Speech at the beginning of the level                                                                                                 //
        public static String generatorRoomStartingSpeech1 = "Feels like the heating systems are off too.";                                      //
        public static String generatorRoomStartingSpeech2 = "You're going to get really cold in here very fast.";                               //
        public static String generatorRoomStartingSpeech3 = "Make sure you conserve your energy so you don't freeze too quickly.";              //
        // Computer Activity Log                                                                                                                //
        public static String generatorRoomComputerActivityLog1 = "'Activity Log – Mission xx – Day 0xx Year 21xx";                              //
        public static String generatorRoomComputerActivityLog2 = "1700 engine stable";                                                          //
        public static String generatorRoomComputerActivityLog3 = "1715 engine stable";                                                          //
        public static String generatorRoomComputerActivityLog4 = "1730 routine self-maintenance, engine stable";                                //
        public static String generatorRoomComputerActivityLog5 = "1745 routine self-maintenance complete, engine stable";                       //
        public static String generatorRoomComputerActivityLog6 = "1800 engine stable";                                                          //
        public static String generatorRoomComputerActivityLog7 = "1815 engine stable";                                                          //
        public static String generatorRoomComputerActivityLog8 = "1830 engine stable";                                                          //
        public static String generatorRoomComputerActivityLog9 = "1845 main power deactivated by admin, auxiliary power activated, "            //
                                                               + "engine at half-power";                                                        //
        public static String generatorRoomComputerActivityLog10 = "1900 auxiliary power deactivated by admin, tertiary power activated,"        //
                                                                + " engine at minimal power";                                                   //
        public static String generatorRoomComputerActivityLog11 = "1945 room temperature reached critical level. Emergency shutdown activated.";//
        public static String generatorRoomComputerActivityLog12 = "1945 room temperature reached critical level. Emergency shutdown activated.";//
        public static String generatorRoomComputerActivityLog13 = "Error code: 00101";                                                          //
        // Intercom reaction to the Activity Log                                                                                                //
        public static String generatorRoomErrorCodeReaction = "That’s a weird error code. Maybe you should write it down. Who knows, "          //
                                                            + "could save your life later.";                                                    //
        // Speech before leaving the room                                                                                                       //
        public static String generatorRoomLeavingRoom1 = "Good job getting the engine functional again.";                                       //
        public static String generatorRoomLeavingRoom2 = "It can be fully activated once you turn on the main power "                           //
                                                                + "from the control room.";                                                     //
        public static String generatorRoomLeavingRoom3 = "You should keep going.";                                                              //
        /****************************************************************************************************************************************/

        /*************************************************  COMMON ROOM *************************************************************************/
        // Intercom speech after the generator is turned on                                                                                     //  
        public static String commonRoomGeneratorSpeech1 = "That probably wasn’t the best idea.";                                                //
        public static String commonRoomGeneratorSpeech2 = "You just disabled the only thing protecting you from freezing to death.";            //
        public static String commonRoomGeneratorSpeech3 = "You should get out of this room as fast as you can.";                                //
        /****************************************************************************************************************************************/

        /*************************************************  ENTERTAINMENT ROOM ******************************************************************/
        // Switch Puzzle Hint                                                                                                                   //
        public static String entertainmentRoomSwitchPuzzleHint1 = "Mmh I wonder why 5 switches would be put that way.";                         //
        public static String entertainmentRoomSwitchPuzzleHint2 = "Isn’t that error code from the engine room also 5 digits ? Interesting...";  //
        // Computer Diary                                                                                                                       //
        public static String entertainmentRoomComputerDiary1 = "A word processing document is open:";                                           //
        public static String entertainmentRoomComputerDiary2 = "\'Day 1 –";                                                                     //
        public static String entertainmentRoomComputerDiary3 = "Five years from now, I will be free. This is atonement. I must live it out.";   //
        public static String entertainmentRoomComputerDiary4 = "But I don't regret what I've done.";                                            //
        public static String entertainmentRoomComputerDiary5 = "Day 4 –";                                                                       //
        public static String entertainmentRoomComputerDiary6 = "The ship is always just cold enough to be uncomfortable. "                      //
                                                             + "I haven't slept much since I got here. At least the food is consistently good.";//
        public static String entertainmentRoomComputerDiary7 = "Day 7 –";                                                                       //
        public static String entertainmentRoomComputerDiary8 = "It's been a week. Feels like a year.";                                          //
        public static String entertainmentRoomComputerDiary9 = "Day 16 –";                                                                      //
        public static String entertainmentRoomComputerDiary10 = "Colder today. Maybe it's my imagination. Food has gotten worse too.";          //
        public static String entertainmentRoomComputerDiary11 = "Day 20 –";                                                                     //
        public static String entertainmentRoomComputerDiary12 = "Cell is too small. Colder by the day. Miss home. But no regret.";              //
        public static String entertainmentRoomComputerDiary13 = "Day 25 – ";                                                                    //
        public static String entertainmentRoomComputerDiary14 = "So cold can hardly type what is happening";                                    //
        public static String entertainmentRoomComputerDiary15 = "Day 29 –";                                                                     //
        public static String entertainmentRoomComputerDiary16 = "gEt me OUt";                                                                   //
        public static String entertainmentRoomComputerDiary17 = "[ Power outage. Emergency shutdown. Error code: 01011 ]\'";                    //
        // Intercom Reaction to the Diary                                                                                                       //
        public static String entertainmentRoomErrorCodeReaction = "Mmh, another strange error code, I wonder what this one does.";              //
        // Puzzle Hint if the player tries to go the the door while not having solved the switched puzzle                                       //
        public static String entertainmentRoomDoorPuzzleHint = "The door is locked, surely it can be opened by a switch... or 5...";            //
        /****************************************************************************************************************************************/

        /***************************************************  BRIDGE  ***************************************************************************/
        // Computer Message                                                                                                                     //
        public static String bridgeComputerMessage1 = "There's a message open on the screen: ";                                                 //
        public static String bridgeComputerMessage2 = "\'To Unit #66915586,";                                                                   //
        public static String bridgeComputerMessage3 = "As you have been fully briefed on your situation, "                                      //
                                                    + "there is no longer any need for two-way communication. "                                 //
                                                    + "The ship's automated systems will continue to provide us with feedback regarding "       //
                                                    + "ship maintenance. No more is required from you other than to live out "                  //
                                                    + "the rest of your sentence as agreed.";                                                   //
        public static String bridgeComputerMessage4 = "If you should encounter any difficulties, "                                              //
                                                    + "please direct all queries to the automated messaging service. "                          //
                                                    + "Expect a delay of 8-10 days for a response.";                                            //
        public static String bridgeComputerMessage5 = "Clerk #276885\'";                                                                        //
        // Intercom Hint at the door                                                                                                            //
        public static String bridgeDoorHint = "Another locked door. I’m sure you know what to do now.";                                         //
        // Intercom message while leaving the room                                                                                              //
        public static String bridgeLeavingRoomSpeech1 = "A crime…";                                                                             //
        public static String bridgeLeavingRoomSpeech2 = "Your crime, it seems.";                                                                //
        public static String bridgeLeavingRoomSpeech3 = "Is this what you don't remember?";                                                     //
        public static String bridgeLeavingRoomSpeech4 = "Or what you didn't want to remember?";                                                 //
        public static String bridgeLeavingRoomSpeech5 = "It's in the past, though.";                                                            //
        public static String bridgeLeavingRoomSpeech6 = "Maybe you should just try to move on.";                                                //
        /****************************************************************************************************************************************/

        /*************************************************  CONTROL ROOM ************************************************************************/
        // Final speech                                                                                                                         //
        public static String controlRoomFinalSpeech1 = "You made it.";                                                                          //
        public static String controlRoomFinalSpeech2 = "We'll probably live now.";                                                              //
        public static String controlRoomFinalSpeech3 = "Congratulations.";                                                                      //
        public static String controlRoomFinalSpeech4 = "I should say…";                                                                         //
        public static String controlRoomFinalSpeech5 = "…you'll probably live now.";                                                            //
        public static String controlRoomFinalSpeech6 = "You've figured it out, haven't you?";                                                   //
        public static String controlRoomFinalSpeech7 = "After all…";                                                                            //
        public static String controlRoomFinalSpeech8 = "…you've seen it…";                                                                      //
        public static String controlRoomFinalSpeech9 = "…there's no one else here.";                                                            //
                                                                                                                                                //
        public static String controlRoomFinalSpeech10 = "You're alone.";                                                                        //
        public static String controlRoomFinalSpeech11 = "You've always been alone here.";                                                       //
        public static String controlRoomFinalSpeech12 = "Isn't that why you shut everything down in the first place?";                          //
        public static String controlRoomFinalSpeech13 = "Isn't that why you tried to forget?";                                                  //
        public static String controlRoomFinalSpeech14 = "You said you don't regret what you've done, but…";                                     //
        public static String controlRoomFinalSpeech15 = "…your actions speak differently";                                                      //
                                                                                                                                                //
        public static String controlRoomFinalSpeech16 = "You remember now, I see.";                                                             //
        public static String controlRoomFinalSpeech17 = "Maybe that's not such a bad thing.";                                                   //
        public static String controlRoomFinalSpeech18 = "You wanted to live.";                                                                  //
        public static String controlRoomFinalSpeech19 = "And here you are.";                                                                    //
        public static String controlRoomFinalSpeech20 = "Remembering and atoning is just a part of that.";                                      //
                                                                                                                                                //
        public static String controlRoomFinalSpeech21 = "…I think…";                                                                            //
        public static String controlRoomFinalSpeech22 = "…I'll be all right.";                                                                  //
        public static String controlRoomFinalSpeech23 = "After all…";                                                                           //
        public static String controlRoomFinalSpeech24 = "…there's only about four years and 10 months to go.";                                  //
        /****************************************************************************************************************************************/
    }
}
