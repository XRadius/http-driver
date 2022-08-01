using HttpDriver;

// TODO: Remove UseKestrel when MemoryController is deleted
Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(x => x
        .UseKestrel(y => y.Limits.MaxRequestBufferSize = y.Limits.MaxRequestLineSize = 2097152)
        .UseStartup<Startup>())
    .UseSystemd()
    .Build().Run();