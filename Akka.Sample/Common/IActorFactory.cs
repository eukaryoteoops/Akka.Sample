﻿using Akka.Actor;
using System;

namespace Akka.Sample.Common
{
    public interface IActorFactory
    {
        IActorRef GetDemoActor();
        IActorRef GetDeployActor();

    }
}
