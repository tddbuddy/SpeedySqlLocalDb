# SpeedySqlLocalDb

A super quick localDB testing framework for use with Entity Framework. It uses a localDB instance for the entire test run's life. Each test's execution is wrapped into transaction to avoid test interfering with each other. This means the cost of setting up the DB is only paid once. That is when the first test executes and the EF migrations are run to initialize the DB to the correct state.

This framework was sliced out of from a production project where the test runs where nearing 40 minutes on the CI server. The bulk of the time was take up by 200 repository integration test. This due to Entity Framework's need to run migrations for each test being executed because each test needed its own DB to allow for proper isolation. After implementing the transactional scoping logic as per this package the total test run time was reduced to 8 minutes. A full 5x speed improvement.

Check out the source at : [https://bitbucket.org/stoneagetechnologies/speedysqllocaldb/src/]

Have a look at the **TddBuddy.SpeedySqlLocalDb.EF.Examples/AttachmentRepositoryTests.cs** for example of how to use it.