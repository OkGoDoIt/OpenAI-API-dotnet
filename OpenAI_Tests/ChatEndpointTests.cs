using Newtonsoft.Json;
using NUnit.Framework;
using OpenAI_API.Chat;
using OpenAI_API.Completions;
using OpenAI_API.Models;
using OpenAI_API.Moderation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OpenAI_Tests
{
	public class ChatEndpointTests
	{
		[SetUp]
		public void Setup()
		{
			OpenAI_API.APIAuthentication.Default = new OpenAI_API.APIAuthentication(Environment.GetEnvironmentVariable("TEST_OPENAI_SECRET_KEY"));
		}

		[Test]
		public void BasicCompletion()
		{
			var api = new OpenAI_API.OpenAIAPI();

			Assert.IsNotNull(api.Chat);

			var results = api.Chat.CreateChatCompletionAsync(new ChatRequest()
			{
				Model = Model.ChatGPTTurbo,
				Temperature = 0.1,
				MaxTokens = 5,
				Messages = new ChatMessage[] {
					new ChatMessage(ChatMessageRole.User, "Hello!")
				}
			}).Result;
			Assert.IsNotNull(results);
			if (results.CreatedUnixTime.HasValue)
			{
				Assert.NotZero(results.CreatedUnixTime.Value);
				Assert.NotNull(results.Created);
				Assert.Greater(results.Created.Value, new DateTime(2018, 1, 1));
				Assert.Less(results.Created.Value, DateTime.Now.AddDays(1));
			}
			else
			{
				Assert.Null(results.Created);
			}
			Assert.NotNull(results.Object);
			Assert.NotNull(results.Choices);
			Assert.NotZero(results.Choices.Count);
			Assert.AreEqual(ChatMessageRole.Assistant, results.Choices[0].Message.Role);
			Assert.That(results.Choices.All(c => c.Message.Role.Equals(ChatMessageRole.Assistant)));
			Assert.That(results.Choices.All(c => c.Message.TextContent.Length > 1));
		}
		[Test]
		public void BasicCompletionWithNames()
		{
			var api = new OpenAI_API.OpenAIAPI();

			Assert.IsNotNull(api.Chat);

			var results = api.Chat.CreateChatCompletionAsync(new ChatRequest()
			{
				Model = Model.ChatGPTTurbo,
				Temperature = 0.1,
				MaxTokens = 5,
				Messages = new ChatMessage[] {
					new ChatMessage(ChatMessageRole.System, "You are the moderator in this workplace chat.  Answer any questions asked of the participants."),
					new ChatMessage(ChatMessageRole.User, "Hello everyone") { Name="John"},
					new ChatMessage(ChatMessageRole.User, "Good morning all")  { Name="Edward"},
					new ChatMessage(ChatMessageRole.User, "Is John here?  Answer yes or no.") { Name = "Cindy" }
					}
			}).Result;
			Assert.IsNotNull(results);
			if (results.CreatedUnixTime.HasValue)
			{
				Assert.NotZero(results.CreatedUnixTime.Value);
				Assert.NotNull(results.Created);
				Assert.Greater(results.Created.Value, new DateTime(2018, 1, 1));
				Assert.Less(results.Created.Value, DateTime.Now.AddDays(1));
			}
			else
			{
				Assert.Null(results.Created);
			}
			Assert.NotNull(results.Object);
			Assert.NotNull(results.Choices);
			Assert.NotZero(results.Choices.Count);
			Assert.AreEqual(ChatMessageRole.Assistant, results.Choices[0].Message.Role);
			Assert.That(results.Choices.All(c => c.Message.Role.Equals(ChatMessageRole.Assistant)));
			Assert.That(results.Choices.All(c => c.Message.TextContent.Length > 1));
			Assert.That(results.ToString().ToLower().Contains("yes"));
		}
		[Test]
		public void SimpleCompletion()
		{
			var api = new OpenAI_API.OpenAIAPI();

			Assert.IsNotNull(api.Chat);

			var results = api.Chat.CreateChatCompletionAsync("Hello!").Result;
			Assert.IsNotNull(results);
			if (results.CreatedUnixTime.HasValue)
			{
				Assert.NotZero(results.CreatedUnixTime.Value);
				Assert.NotNull(results.Created);
				Assert.Greater(results.Created.Value, new DateTime(2018, 1, 1));
				Assert.Less(results.Created.Value, DateTime.Now.AddDays(1));
			}
			else
			{
				Assert.Null(results.Created);
			}
			Assert.NotNull(results.Object);
			Assert.NotNull(results.Choices);
			Assert.NotZero(results.Choices.Count);
			Assert.AreEqual(ChatMessageRole.Assistant, results.Choices[0].Message.Role);
			Assert.That(results.Choices.All(c => c.Message.Role.Equals(ChatMessageRole.Assistant)));
			Assert.That(results.Choices.All(c => c.Message.Role == ChatMessageRole.Assistant));
			Assert.That(results.Choices.All(c => c.Message.TextContent.Length > 1));
			Assert.IsNotEmpty(results.ToString());
		}

		[TestCase("gpt-3.5-turbo")]
		[TestCase("gpt-4")]
		[TestCase("gpt-4-1106-preview")]
		public void ChatBackAndForth(string model)
		{
			var api = new OpenAI_API.OpenAIAPI();

			var chat = api.Chat.CreateConversation();
			chat.Model = model;
			chat.RequestParameters.Temperature = 0;

			chat.AppendSystemMessage("You are a teacher who helps children understand if things are animals or not.  If the user tells you an animal, you say \"yes\".  If the user tells you something that is not an animal, you say \"no\".  You only ever respond with \"yes\" or \"no\".  You do not say anything else.");
			chat.AppendUserInput("Is this an animal? Cat");
			chat.AppendExampleChatbotOutput("Yes");
			chat.AppendUserInput("Is this an animal? House");
			chat.AppendExampleChatbotOutput("No");
			chat.AppendUserInput("Is this an animal? Dog");
			string res = chat.GetResponseFromChatbotAsync().Result;
			Assert.NotNull(res);
			Assert.IsNotEmpty(res);
			Assert.AreEqual("Yes", res.Trim());
			chat.AppendUserInput("Is this an animal? Chair");
			res = chat.GetResponseFromChatbotAsync().Result;
			Assert.NotNull(res);
			Assert.IsNotEmpty(res);
			Assert.AreEqual("No", res.Trim());
		}

		[TestCase(false)]
		[TestCase(true)]
		[TestCase(null)]
		public void ChatTooLong(bool? addTruncationHandler)
		{
			var api = new OpenAI_API.OpenAIAPI();

			var chat = api.Chat.CreateConversation();
			chat.Model = "gpt-3.5-turbo-0613";
			chat.RequestParameters.Temperature = 0;

			if (addTruncationHandler == true)
			{
				chat.OnTruncationNeeded += (sender, args) =>
				{
					for (int i = 0; i < args.Count; i++)
					{
						if (args[i].Role != ChatMessageRole.System)
						{
							args.RemoveAt(i);
							return;
						}
					}
				};
			}
			else if (addTruncationHandler == false)
			{
				chat.AutoTruncateOnContextLengthExceeded = false;
			}

			AddLongExampleToChat(chat);

			if (addTruncationHandler == false)
			{
				// it should fail because the response is too long
				Assert.Throws<AggregateException>(() => chat.GetResponseFromChatbotAsync().Wait());
			}
			else
			{
				string res = chat.GetResponseFromChatbotAsync().Result;
				Assert.IsTrue(res.Contains("Rayleigh"));
				Assert.AreEqual(3260,chat.MostRecentApiResult.Usage.PromptTokens);
			}
		}


		[TestCase(false)]
		[TestCase(true)]
		[TestCase(null)]
		public async Task ChatTooLongStreaming(bool? addTruncationHandler)
		{
			var api = new OpenAI_API.OpenAIAPI();

			var chat = api.Chat.CreateConversation();
			chat.Model = "gpt-3.5-turbo-0613";
			chat.RequestParameters.Temperature = 0;

			if (addTruncationHandler == true)
			{
				chat.OnTruncationNeeded += (sender, args) =>
				{
					for (int i = 0; i < args.Count; i++)
					{
						if (args[i].Role != ChatMessageRole.System)
						{
							args.RemoveAt(i);
							return;
						}
					}
				};
			}
			else if (addTruncationHandler == false)
			{
				chat.AutoTruncateOnContextLengthExceeded = false;
			}

			AddLongExampleToChat(chat);

			if (addTruncationHandler == false)
			{
				// it should fail because the response is too long
				Assert.Throws<AggregateException>(() => chat.StreamResponseFromChatbotAsync(s=>s.ToString()).Wait());
			}
			else
			{
				string result = "";
				int streamParts = 0;

				await foreach (var streamResultPart in chat.StreamResponseEnumerableFromChatbotAsync())
				{
					result += streamResultPart;
					streamParts++;
				}

				Assert.NotNull(result);
				Assert.IsNotEmpty(result);
				Assert.That(result.Contains("Rayleigh"));
				Assert.Greater(result.Length, 200);
				Assert.Greater(streamParts, 5);

				Assert.AreEqual(ChatMessageRole.User, chat.Messages.Last().Role);
				Assert.AreEqual(result, chat.Messages.Last().TextContent);
			}
		}

		/// <summary>
		/// This adds a very long chat history that exceeds the prompt token limit
		/// </summary>
		/// <param name="chat">The chat to add the examples to</param>
		private static void AddLongExampleToChat(Conversation chat)
		{
			chat.AppendSystemMessage("You are a helpful assistant who is really good at explaining things to students.");

			chat.AppendUserInput("Please explain how mountains are formed in great detail");

			// the following example text is taken from wikipedia
			chat.AppendExampleChatbotOutput(@"Mountain formation refers to the geological processes that underlie the formation of mountains. These processes are associated with large-scale movements of the Earth's crust (tectonic plates). Folding, faulting, volcanic activity, igneous intrusion and metamorphism can all be parts of the orogenic process of mountain building. The formation of mountains is not necessarily related to the geological structures found on it.
The understanding of specific landscape features in terms of the underlying tectonic processes is called tectonic geomorphology, and the study of geologically young or ongoing processes is called neotectonics.
From the late 18th century until its replacement by plate tectonics in the 1960s, geosyncline theory was used to explain much mountain-building.
Types of mountains
See also: List of mountain types
There are five main types of mountains: volcanic, fold, plateau, fault-block and dome. A more detailed classification useful on a local scale predates plate tectonics and adds to these categories.
Volcanic mountains
Annotated view includes Ushkovsky, Tolbachik, Bezymianny, Zimina, and Udina stratovolcanoes of Kamchatka, Russia. Oblique view taken on November 12, 2013, from ISS.
Stratovolcanoes associated with a subduction zone (left) and a spreading ridge volcano (right). A hotspot volcano is center.
Movements of tectonic plates create volcanoes along the plate boundaries, which erupt and form mountains. A volcanic arc system is a series of volcanoes that form near a subduction zone where the crust of a sinking oceanic plate melts and drags water down with the subducting crust.
The Dome of Vitosha mountain next to Sofia
Most volcanoes occur in a band encircling the Pacific Ocean (the Pacific Ring of Fire), and in another that extends from the Mediterranean across Asia to join the Pacific band in the Indonesian Archipelago. The most important types of volcanic mountain are composite cones or stratovolcanoes (Vesuvius, Kilimanjaro and Mount Fuji are examples) and shield volcanoes (such as Mauna Loa on Hawaii, a hotspot volcano).
A shield volcano has a gently sloping cone due to the low viscosity of the emitted material, primarily basalt. Mauna Loa is the classic example, with a slope of 4°-6°. (The relation between slope and viscosity falls under the topic of angle of repose. Vitosha - the domed mountain next to Sofia, capital of Bulgaria, is also formed by volcanic activity.
Zard-Kuh, a fold mountain in the central Zagros range of Iran.
When plates collide or undergo subduction (that is – ride one over another), the plates tend to buckle and fold, forming mountains. Most of the major continental mountain ranges are associated with thrusting and folding or orogenesis. Examples are the Balkan Mountains, the Jura and the Zagros mountains.
Sierra Nevada Mountains (formed by delamination) as seen from the International Space Station.
When a fault block is raised or tilted, block mountains can result. Higher blocks are called horsts and troughs are called grabens. A spreading apart of the surface causes tensional forces. When the tensional forces are strong enough to cause a plate to split apart, it does so such that a center block drops down relative to its flanking blocks.
An example of this is the Sierra Nevada Range, where delamination created a block 650 km long and 80 km wide that consists of many individual portions tipped gently west, with east facing slips rising abruptly to produce the highest mountain front in the continental United States.
Another good example is the Rila - Rhodope mountain Massif in Bulgaria, Southeast Europe, including the well defined horsts of Belasitsa (linear horst), Rila mountain (vaulted domed shaped horst) and Pirin mountain - a horst forming a massive anticline situated between the complex graben valleys of Struma and that of Mesta.
Uplifted passive margins
Unlike orogenic mountains there is no widely accepted geophysical model that explains elevated passive continental margins such as the Scandinavian Mountains, Eastern Greenland, the Brazilian Highlands or Australia's Great Dividing Range.
Models
See also: Extensional tectonics, Rift valley, Rift, Prediction of volcanic activity, and Geomorphology
Hotspot volcanoes
Hotspots are supplied by a magma source in the Earth's mantle called a mantle plume. Although originally attributed to a melting of subducted oceanic crust, recent evidence belies this connection. The mechanism for plume formation remains a research topic.
Fault blocks
Several movements of the Earth's crust that lead to mountains are associated with faults. These movements actually are amenable to analysis that can predict, for example, the height of a raised block and the width of an intervening rift between blocks using the rheology of the layers and the forces of isostasy. Early bent plate models predicting fractures and fault movements have evolved into today's kinematic and flexural models.");
			chat.AppendUserInput("Explain how a car engine works in great detail");

			// the following example text is taken from wikipedia
			chat.AppendExampleChatbotOutput(@"There are a wide variety of propulsion systems available or potentially available for automobiles and other vehicles. Options included internal combustion engines fueled by petrol, diesel, propane, or natural gas; hybrid vehicles, plug-in hybrids, fuel cell vehicles fueled by hydrogen and all electric cars. Fueled vehicles seem to have the advantage due to the limited range and high cost of batteries. Some options required construction of a network of fueling or charging stations. With no compelling advantage for any particular option, car makers pursued parallel development tracks using a variety of options. Reducing the weight of vehicles was one strategy being employed.
Recent developments
The use of high-technology (such as electronic engine control units) in advanced designs resulting from substantial investments in development research by European countries and Japan seemed to give an advantage to them over Chinese automakers and parts suppliers who, as of 2013, had low development budgets and lacked capacity to produce parts for high-tech engine and power train designs.
Characteristics
The chief characteristic of an automotive engine (compared to a stationary engine or a marine engine) is a high power-to-weight ratio. This is achieved by using a high rotational speed. However, automotive engines are sometimes modified for marine use, forming a marine automobile engine.
History
Main article: History of the automobile
In the early years, steam engines and electric motors were tried, but with limited success. In the 20th century, the internal combustion (ic) engine became dominant. In 2015, the internal combustion engine remains the most widely used but a resurgence of electricity seems likely because of increasing concern about ic engine exhaust gas emissions.
As of 2017, the majority of the cars in the United States are gasoline powered. In the early 1900s, the internal combustion engines faced competition from steam and electric engines. The internal combustion engines of the time was powered by gasoline. Internal combustion engines function with the concept of a piston being pushed by the pressure of a certain explosion. This is the predecessor to the modern diesel engine used in automobiles, but more specifically, heavy duty vehicles such as semi-trucks.
Engine types
Internal combustion engines
Main article: Internal combustion engine
Petrol engines quickly became the choice of manufacturers and consumers alike. Despite the rough start, noisy and dirty engine, and the difficult gear shifting, new technologies such as the production line and the advancement of the engine allowed the standard production of the gas automobiles. This is the start, from the invention of the gas automobile in 1876, to the beginning of mass production in the 1890s. Henry Ford's Model T drove down the price of cars to a more affordable price. At the same time, Charles Kettering invented an electric starter, allowing the car to be more efficient than the mechanical starter.
An internal combustion engine is a motor that is powered by the expansion of gas which is created by the combustion of hydrocarbon gases fuels. Gasoline engines became popular as a result of this, as internal combustion engines were commonly known as gasoline engines. Although gasoline engines became popular, they were not particularly desirable due to the dangers of fuel leaks that may cause explosions. Therefore, many inventors attempted to create a kerosene burning engine as a result. This was not a successful venture applying it for automotive usage. There are many different types of fuels for internal combustion engines. These include diesel, gasoline, and ethanol.
Steam engines
Main article: History of steam road vehicles
The steam engine was invented in the late 1700s, and the primary method of powering engines and soon, locomotives. One of the most popular steam automobile was the “Stanley Steamer,” offering low pollution, power, and speed. The downside of these steam automobiles is the unreliability, complexity, and the frequent accidents that occurred with them. The startup time for a steam car may take up to 45 minutes, defeating the purpose of faster transportation. By the time the steam automobile was improved, the complexity of manufacturing relative to the gas automobiles made steam automobiles unprofitable.
A steam engine is a device which transforms heat into mechanical motion. This is provided with the usage of boilers, which create steam by boiling water. In the early 1900s, Abner Doble introduced a steam-powered car in the United States which had capabilities that could potentially overpower Ford's Model T in efficiency.
Electric motors
Main article: Electric motor
Electric vehicles seemed to be the most viable option, similar to the steam automobiles. It was first invented in the early 1800s, and became a viable option of transportation around 1890, when William Morrison created the first electric car that traveled 14 miles per hour. The electric cars offered low pollution and a soundless ride, unlike their gasoline counterparts. The greatest downside of electric cars was the range. The typical electric car could reach around 20 miles before requiring a recharge. Manufacturers could not increase the number of batteries, due to the bulkiness of the batteries at the time. Without an incentive to purchase the electric automobiles, gas automobiles was the most viable option at the time.
Electric cars use batteries to store electricity which is used to power electric motors. The battery delivers the power to the motor, which is either AC or DC. The difference between AC and DC motors is the sort of system that is required to run it in an electric vehicle. An AC motor is generally cheaper but the components required to run it in an electric vehicle such as the controller and inverter makes it more expensive than the DC motor. A unique feature of electric vehicles compared to its gasoline counterparts, the electric vehicle is more simple than the gasoline vehicle.
In the 1970s, the electric vehicle made its reappearance because of the 1973 OPEC Oil Embargo. Previously, the abundant gasoline had become the prime source of fuel for vehicles. But after the shortage, manufacturers began looking towards electric vehicles again. Despite the improved technology from the 1800s, the electric vehicles faced similar technological flaws such as limited mileage and speed. They could only travel up to 45 miles per hour and had a range of approximately 40 miles.");
			chat.AppendUserInput("Explain airplanes in great detail");
			// the following example text is taken from wikipedia
			chat.AppendExampleChatbotOutput(@"An airplane (American English), or aeroplane (Commonwealth English), informally plane, is a fixed-wing aircraft that is propelled forward by thrust from a jet engine, propeller, or rocket engine. Airplanes come in a variety of sizes, shapes, and wing configurations. The broad spectrum of uses for airplanes includes recreation, transportation of goods and people, military, and research. Worldwide, commercial aviation transports more than four billion passengers annually on airliners Most airplanes are flown by a pilot on board the aircraft, but some are designed to be remotely or computer-controlled such as drones.
The Wright brothers invented and flew the first airplane in 1903, recognized as ""the first sustained and controlled heavier-than-air powered flight"". Following its limited use in World War I, aircraft technology continued to develop. Airplanes had a presence in all the major battles of World War II. The first jet aircraft was the German Heinkel He 178 in 1939. The first jet airliner, the de Havilland Comet, was introduced in 1952. The Boeing 707, the first widely successful commercial jet, was in commercial service for more than 50 years, from 1958 to at least 2013.
Etymology and usage
First attested in English in the late 19th century (prior to the first sustained powered flight), the word airplane, like aeroplane, derives from the French aéroplane, which comes from the Greek ἀήρ (aēr), ""air"" In an example of synecdoche, the word for the wing came to refer to the entire aircraft.
In the United States and Canada, the term ""airplane"" is used for powered fixed-wing aircraft. In the United Kingdom and Ireland and most of the Commonwealth, the term ""aeroplane"" (/ˈɛərəpleɪn/) is usually applied to these aircraft.
History
Main articles: Aviation history and First flying machine
Le Bris and his glider, Albatros II, photographed by Nadar, 1868
Otto Lilienthal in mid-flight, c. 1895
Antecedents
Many stories from antiquity involve flight, such as the Greek legend of Icarus and Daedalus, and the Vimana in ancient Indian epics. Around 400 BC in Greece, Archytas was reputed to have designed and built the first artificial, self-propelled flying device, a bird-shaped model propelled by a jet of what was probably steam, said to have flown some 200 m (660 ft).
Some of the earliest recorded attempts with gliders were those by the 9th-century Andalusian and Arabic-language poet Abbas ibn Firnas and the 11th-century English monk Eilmer of Malmesbury; both experiments injured their pilots. Leonardo da Vinci researched the wing design of birds and designed a man-powered aircraft in his Codex on the Flight of Birds (1502), noting for the first time the distinction between the center of mass and the center of pressure of flying birds.
In 1799, George Cayley set forth the concept of the modern airplane as a fixed-wing flying machine with separate systems for lift, propulsion, and control. Other aviators who made similar flights at that time were Otto Lilienthal, Percy Pilcher, and Octave Chanute.
Sir Hiram Maxim built a craft that weighed 3.5 tons, with a 110-foot (34 m) wingspan that was powered by two 360-horsepower (270 kW) steam engines driving two propellers. In 1894, his machine was tested with overhead rails to prevent it from rising. The test showed that it had enough lift to take off. The craft was uncontrollable and it is presumed that Maxim realized this because he subsequently abandoned work on it.
In the 1890s, Lawrence Hargrave conducted research on wing structures and developed a box kite that lifted the weight of a man. His box kite designs were widely adopted. Although he also developed a type of rotary aircraft engine, he did not create and fly a powered fixed-wing aircraft.
Between 1867 and 1896, the German pioneer of human aviation Otto Lilienthal developed heavier-than-air flight. He was the first person to make well-documented, repeated, successful gliding flights. Lilienthal's work led to him developing the concept of the modern wing,
Early powered flights
Patent drawings of Clement Ader's Éole.
The Frenchman Clement Ader constructed his first of three flying machines in 1886, the Éole. It was a bat-like design run by a lightweight steam engine of his own invention, with four cylinders developing 20 horsepower (15 kW), driving a four-blade propeller. The engine weighed no more than 4 kilograms per kilowatt (6.6 lb/hp). The wings had a span of 14 m (46 ft). All-up weight was 300 kilograms (660 lb). On 9 October 1890, Ader attempted to fly the Éole. Aviation historians give credit to this effort as a powered take-off and uncontrolled hop of approximately 50 m (160 ft) at a height of approximately 200 mm (7.9 in).
The American Wright brothers flights in 1903 are recognized by the Fédération Aéronautique Internationale (FAI), the standard-setting and record-keeping body for aeronautics, as ""the first sustained and controlled heavier-than-air powered flight"". By 1905, the Wright Flyer III was capable of fully controllable, stable flight for substantial periods. The Wright brothers credited Otto Lilienthal as a major inspiration for their decision to pursue manned flight.
Santos-Dumont 14-bis, between 1906 and 1907
In 1906, the Brazilian Alberto Santos-Dumont made what was claimed to be the first airplane flight unassisted by catapult
An early aircraft design that brought together the modern monoplane tractor configuration was the Blériot VIII design of 1908. It had movable tail surfaces controlling both yaw and pitch, a form of roll control supplied either by wing warping or by ailerons and controlled by its pilot with a joystick and rudder bar. It was an important predecessor of his later Blériot XI Channel-crossing aircraft of the summer of 1909.
World War I served as a testbed for the use of the airplane as a weapon. Airplanes demonstrated their potential as mobile observation platforms, then proved themselves to be machines of war capable of causing casualties to the enemy. The earliest known aerial victory with a synchronized machine gun-armed fighter aircraft occurred in 1915, by German Luftstreitkräfte Leutnant Kurt Wintgens. Fighter aces appeared; the greatest (by number of Aerial Combat victories) was Manfred von Richthofen.
Following WWI, aircraft technology continued to develop. Alcock and Brown crossed the Atlantic non-stop for the first time in 1919. The first international commercial flights took place between the United States and Canada in 1919.
Airplanes had a presence in all the major battles of World War II. They were an essential component of the military strategies of the period, such as the German Blitzkrieg, The Battle of Britain, and the American and Japanese aircraft carrier campaigns of the Pacific War.
Development of jet aircraft
The Concorde supersonic transport aircraft
The first practical jet aircraft was the German Heinkel He 178, which was tested in 1939. In 1943, the Messerschmitt Me 262, the first operational jet fighter aircraft, went into service in the German Luftwaffe.
The first jet airliner, the de Havilland Comet, was introduced in 1952. The Boeing 707, the first widely successful commercial jet, was in commercial service for more than 50 years, from 1958 to 2010. The Boeing 747 was the world's biggest passenger aircraft from 1970 until it was surpassed by the Airbus A380 in 2005.
Supersonic airliner flights, including those of the Concorde, have been limited to over-water flight at supersonic speed because of their sonic boom, which is prohibited over most populated land areas. The high cost of operation per passenger-mile and a deadly crash in 2000 induced the operators of the Concorde to remove it from service.
Propulsion
Propeller
An Antonov An-2 biplane
An aircraft propeller, or airscrew, converts rotary motion from an engine or other power source, into a swirling slipstream which pushes the propeller forwards or backwards. It comprises a rotating power-driven hub, to which are attached two or more radial airfoil-section blades such that the whole assembly rotates about a longitudinal axis.
Reciprocating engine
Reciprocating engines in aircraft have three main variants, radial, in-line and flat or horizontally opposed engine. The radial engine is a reciprocating type internal combustion engine configuration in which the cylinders ""radiate"" outward from a central crankcase like the spokes of a wheel and was commonly used for aircraft engines before gas turbine engines became predominant. An inline engine is a reciprocating engine with banks of cylinders, one behind another, rather than rows of cylinders, with each bank having any number of cylinders, but rarely more than six, and may be water-cooled. A flat engine is an internal combustion engine with horizontally-opposed cylinders.");

			chat.AppendUserInput("Explain why the sky is blue in great detail");
		}

		[TestCase("gpt-3.5-turbo")]
		[TestCase("gpt-4")]
		[TestCase("gpt-4-1106-preview")]
		public void ChatWithNames(string model)
		{
			var api = new OpenAI_API.OpenAIAPI();

			var chat = api.Chat.CreateConversation();
			chat.Model = model;
			chat.RequestParameters.Temperature = 0;

			chat.AppendSystemMessage("You are the moderator in this workplace chat.  Answer any questions asked of the participants.");
			chat.AppendUserInputWithName("John", "Hello everyone");
			chat.AppendUserInputWithName("Edward", "Good morning all");
			chat.AppendUserInputWithName("Cindy", "Is John here?  Answer yes or no.");
			chat.AppendExampleChatbotOutput("Yes");
			chat.AppendUserInputWithName("Cindy", "Is Monica here?  Answer yes or no.");
			string res = chat.GetResponseFromChatbotAsync().Result;
			Assert.NotNull(res);
			Assert.IsNotEmpty(res);
			Assert.That(res.ToLower().Contains("no"));
			chat.AppendUserInputWithName("Cindy", "Is Edward here?  Answer yes or no.");
			res = chat.GetResponseFromChatbotAsync().Result;
			Assert.NotNull(res);
			Assert.IsNotEmpty(res);
			Assert.That(res.ToLower().Contains("yes"));
		}


		[TestCase("gpt-3.5-turbo")]
		[TestCase("gpt-4-1106-preview")]
		public async Task StreamCompletionEnumerableAsync_ShouldStreamData(string model)
		{
			var api = new OpenAI_API.OpenAIAPI();

			Assert.IsNotNull(api.Chat);

			var req = new ChatRequest()
			{
				Model = model,
				Temperature = 0.2,
				MaxTokens = 500,
				Messages = new ChatMessage[] {
					new ChatMessage(ChatMessageRole.User, "Please explain how mountains are formed in great detail.")
				}
			};

			var chatResults = new List<ChatResult>();
			await foreach (var res in api.Chat.StreamChatEnumerableAsync(req))
			{
				chatResults.Add(res);
			}

			Assert.Greater(chatResults.Count, 100);
			Assert.That(chatResults.Select(cr => cr.Choices[0].Delta.TextContent).Count(c => !string.IsNullOrEmpty(c)) > 50);
		}

		[TestCase("gpt-3.5-turbo")]
		[TestCase("gpt-4-1106-preview")]
		public async Task StreamingConversation(string model)
		{
			var api = new OpenAI_API.OpenAIAPI();

			var chat = api.Chat.CreateConversation();
			chat.RequestParameters.MaxTokens = 500;
			chat.RequestParameters.Temperature = 0.2;
			chat.Model = model;

			chat.AppendSystemMessage("You are a helpful assistant who is really good at explaining things to students.");
			chat.AppendUserInput("Please explain to me how mountains are formed in great detail.");

			string result = "";
			int streamParts = 0;

			await foreach (var streamResultPart in chat.StreamResponseEnumerableFromChatbotAsync())
			{
				result += streamResultPart;
				streamParts++;
			}

			Assert.NotNull(result);
			Assert.IsNotEmpty(result);
			Assert.That(result.ToLower().Contains("mountains"));
			Assert.Greater(result.Length, 200);
			Assert.Greater(streamParts, 5);

			Assert.AreEqual(ChatMessageRole.User, chat.Messages.Last().Role);
			Assert.AreEqual(result, chat.Messages.Last().TextContent);
		}

		[TestCase("gpt-4-1106-preview")]
		[TestCase("gpt-3.5-turbo-1106")]
		public void ChatJsonFormat(string model)
		{
			var api = new OpenAI_API.OpenAIAPI();
			ChatRequest chatRequest = new ChatRequest()
			{
				Model = model,
				Temperature = 0.0,
				MaxTokens = 500,
				ResponseFormat = ChatRequest.ResponseFormats.JsonObject,
				Messages = new ChatMessage[] {
					new ChatMessage(ChatMessageRole.System, "You are a helpful assistant designed to output JSON."),
					new ChatMessage(ChatMessageRole.User, "Who won the world series in 2020?  Return JSON of a 'wins' dictionary with the year as the numeric key and the winning team as the string value.")
				}
			};

			var results = api.Chat.CreateChatCompletionAsync(chatRequest).Result;
			Assert.IsNotNull(results);
			
			Assert.NotNull(results.Object);
			Assert.NotNull(results.Choices);
			Assert.NotZero(results.Choices.Count);
			Assert.AreEqual(ChatMessageRole.Assistant, results.Choices[0].Message.Role);
			Assert.That(results.Choices.All(c => c.Message.TextContent.Length > 1));
			Assert.AreEqual("stop", results.Choices[0].FinishReason);

			using (StringReader stringReader = new StringReader(results.Choices[0].Message.TextContent))
			{
				using (JsonTextReader jsonReader= new JsonTextReader(stringReader))
				{
					var serializer = new JsonSerializer();
					var json = serializer.Deserialize<Dictionary<string, Dictionary<int, string>>>(jsonReader);
					Assert.NotNull(json);
					Assert.IsTrue(json.ContainsKey("wins"));
					Assert.IsTrue(json["wins"].ContainsKey(2020));
					Assert.AreEqual("Los Angeles Dodgers", json["wins"][2020]);
				}
			}

		}
	}
}
