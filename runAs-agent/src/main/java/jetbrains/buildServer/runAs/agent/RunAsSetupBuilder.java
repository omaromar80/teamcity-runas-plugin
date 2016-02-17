package jetbrains.buildServer.runAs.agent;

import java.io.File;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.List;
import jetbrains.buildServer.dotNet.buildRunner.agent.*;
import jetbrains.buildServer.runAs.common.Constants;
import jetbrains.buildServer.util.StringUtil;
import org.jetbrains.annotations.NotNull;

public class RunAsSetupBuilder implements CommandLineSetupBuilder {
  static final String TOOL_FILE_NAME = "runAs.cmd";
  static final String CREDENTIALS_EXT = ".cred";
  static final String CMD_EXT = ".cmd";
  private final RunnerParametersService myParametersService;
  private final BuildFeatureParametersService myBuildFeatureParametersService;
  private final FileService myFileService;
  private final ResourcePublisher mySettingsPublisher;
  private final ResourceGenerator<CredentialsSettings> myCredentialsGenerator;
  private final ResourceGenerator<RunAsCmdSettings> myRunAsCmdGenerator;
  private final TextParser<List<EnvironmentVariable>> myEnvironmentVariablesParser;
  private final ResourceGenerator<List<EnvironmentVariable>> myEnvVarsCmdGenerator;
  private final CommandLineArgumentsService myCommandLineArgumentsService;

  public RunAsSetupBuilder(
    @NotNull final RunnerParametersService parametersService,
    @NotNull final BuildFeatureParametersService buildFeatureParametersService,
    @NotNull final FileService fileService,
    @NotNull final ResourcePublisher settingsPublisher,
    @NotNull final ResourceGenerator<CredentialsSettings> credentialsGenerator,
    @NotNull final ResourceGenerator<RunAsCmdSettings> runAsCmdGenerator,
    @NotNull final TextParser<List<EnvironmentVariable>> environmentVariablesParser,
    @NotNull final ResourceGenerator<List<EnvironmentVariable>> envVarsCmdGenerator,
    @NotNull final CommandLineArgumentsService commandLineArgumentsService) {
    myParametersService = parametersService;
    myBuildFeatureParametersService = buildFeatureParametersService;
    myFileService = fileService;
    mySettingsPublisher = settingsPublisher;
    myCredentialsGenerator = credentialsGenerator;
    myRunAsCmdGenerator = runAsCmdGenerator;
    myEnvironmentVariablesParser = environmentVariablesParser;
    myEnvVarsCmdGenerator = envVarsCmdGenerator;
    myCommandLineArgumentsService = commandLineArgumentsService;
  }

  @NotNull
  @Override
  public Iterable<CommandLineSetup> build(@NotNull final CommandLineSetup commandLineSetup) {
    if(!myParametersService.isRunningUnderWindows()) {
      return Collections.singleton(commandLineSetup);
    }

    // Get parameters
    final List<String> userNames = myBuildFeatureParametersService.getBuildFeatureParameters(Constants.BUILD_FEATURE_TYPE, Constants.USER_VAR);
    final List<String> passwords = myBuildFeatureParametersService.getBuildFeatureParameters(Constants.BUILD_FEATURE_TYPE, Constants.PASSWORD_VAR);
    if(userNames.size() == 0 || passwords.size() == 0) {
      return Collections.singleton(commandLineSetup);
    }

    final String userName = userNames.get(0);
    final String password = passwords.get(0);
    if(StringUtil.isEmptyOrSpaces(userName) || password == null) {
      return Collections.singleton(commandLineSetup);
    }

    // Resources
    final ArrayList<CommandLineResource> resources = new ArrayList<CommandLineResource>();
    resources.addAll(commandLineSetup.getResources());

    // Credentials
    final File credentialsFile = myFileService.getTempFileName(CREDENTIALS_EXT);
    resources.add(new CommandLineFile(mySettingsPublisher, credentialsFile.getAbsoluteFile(), myCredentialsGenerator.create(new CredentialsSettings(userName, password))));

    // Environment variables
    //noinspection SpellCheckingInspection
    final List<String> noninheritableEnvironmentVariables = myBuildFeatureParametersService.getBuildFeatureParameters(Constants.BUILD_FEATURE_TYPE, Constants.NONINHERITABLE_ENVIRONMENT_VARIABLES);
    final List<EnvironmentVariable> environmentVariables = new ArrayList<EnvironmentVariable>();
    if(noninheritableEnvironmentVariables.size() > 0) {
      environmentVariables.addAll(myEnvironmentVariablesParser.parse(noninheritableEnvironmentVariables.get(0)));
    }

    final File envVarsCmdFile = myFileService.getTempFileName(CMD_EXT);
    resources.add(new CommandLineFile(mySettingsPublisher, envVarsCmdFile.getAbsoluteFile(), myEnvVarsCmdGenerator.create(environmentVariables)));

    // Command
    List<CommandLineArgument> cmdLineArgs = new ArrayList<CommandLineArgument>();
    cmdLineArgs.add(new CommandLineArgument(commandLineSetup.getToolPath(), CommandLineArgument.Type.PARAMETER));
    cmdLineArgs.addAll(commandLineSetup.getArgs());

    final RunAsCmdSettings runAsCmdSettings = new RunAsCmdSettings(
      myCommandLineArgumentsService.createCommandLineString(cmdLineArgs),
      myFileService.getCheckoutDirectory().getAbsolutePath());

    final File cmdFile = myFileService.getTempFileName(CMD_EXT);
    resources.add(new CommandLineFile(mySettingsPublisher, cmdFile.getAbsoluteFile(), myRunAsCmdGenerator.create(runAsCmdSettings)));

    return Collections.singleton(
      new CommandLineSetup(
        getTool().getAbsolutePath(),
        Arrays.asList(
          new CommandLineArgument(credentialsFile.getAbsolutePath(), CommandLineArgument.Type.PARAMETER),
          new CommandLineArgument(envVarsCmdFile.getAbsolutePath(), CommandLineArgument.Type.PARAMETER),
          new CommandLineArgument(cmdFile.getAbsolutePath(), CommandLineArgument.Type.PARAMETER)),
        resources));
  }

  private File getTool() {
    final File path = new File(myParametersService.getToolPath(Constants.RUN_AS_TOOL_NAME), TOOL_FILE_NAME);
    myFileService.validatePath(path);
    return path;
  }
}