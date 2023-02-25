﻿using Xunit;
using System;
using Microsoft.Extensions.Configuration;
using FluentAssertions;

namespace Addon.Episerver.EnvironmentSynchronizer.Configuration.Tests
{
	public class ConfigurationReaderTests
	{
		[Fact]
		public void ReadConfiguration_Null_Settings_Test()
		{
			// Arrange

			// Act
			var config = new ConfigurationBuilder()
				 .AddEnvironmentVariables()
				 .Build();

			// Assert
			config.Should().NotBeNull();
		}

		[Fact()]
		public void ReadConfiguration_SiteDefinition_All_Settings_Test()
		{
			// Arrange
			var configName = "all-settings.json";

			// Act
			var options = GetConfiguration(configName);

			//// Assert
			options.Should().NotBeNull();

			// Site Definitions
			options.SiteDefinitions.Should().NotBeNull();
			options.SiteDefinitions.Should().HaveCount(1);
			options.SiteDefinitions[0].Id.Should().Be("6AAEAF2F-20F9-41EB-8260-D0BBA76DB141");
			options.SiteDefinitions[0].Name.Should().Be("CustomerX");
			options.SiteDefinitions[0].SiteUrl.Should().Be("https://custxmstr972znb5prep.azurewebsites.net/");
			options.SiteDefinitions[0].Hosts.Should().HaveCount(2);

			options.SiteDefinitions[0].Hosts[0].Name.Should().Be("*");
			options.SiteDefinitions[0].Hosts[0].Language.Should().Be("en");
			options.SiteDefinitions[0].Hosts[0].UseSecureConnection.Should().BeFalse();

			// Scheduled jobs
			options.ScheduledJobs.Should().NotBeNull();
			options.ScheduledJobs.Should().HaveCount(2);

			options.ScheduledJobs[0].Id.Should().Be("*");
			options.ScheduledJobs[0].Name.Should().Be("*");
			options.ScheduledJobs[0].IsEnabled.Should().BeFalse();
			options.ScheduledJobs[0].AutoRun.Should().BeFalse();

			options.ScheduledJobs[1].Id.Should().BeNull();
			options.ScheduledJobs[1].Name.Should().Be("YourScheduledJob");
			options.ScheduledJobs[1].IsEnabled.Should().BeTrue();
			options.ScheduledJobs[1].AutoRun.Should().BeTrue();
		}

		[Fact()]
		public void ReadConfiguration_SiteDefinition_No_Settings_Test()
		{
			// Arrange
			var configName = "no-settings.json";


			// Act
			var options = GetConfiguration(configName);

			// Assert
			options.Should().NotBeNull();

			options.RunAsInitializationModule.Should().BeFalse();
			options.RunInitializationModuleEveryStartup.Should().BeFalse();

			options.SiteDefinitions.Should().BeNull();
			options.ScheduledJobs.Should().BeNull();
		}

		public static EnvironmentSynchronizerOptions GetConfiguration(string name)
		{
			var path = Environment.CurrentDirectory + "\\test-configs";
			var configuration = new EnvironmentSynchronizerOptions();

			new ConfigurationBuilder()
				.SetBasePath(path)
				.AddJsonFile(name, optional: true)
				.Build()
				.GetSection(EnvironmentSynchronizerOptions.EnvironmentSynchronizer)
				.Bind(configuration);

			return configuration;
		}
	}
}