﻿using Microsoft.Extensions.Logging;
using Optsol.Components.Domain.Services.Push;
using Optsol.Components.Infra.Firebase.Models;
using AutoMapper;
using System;
using System.Threading.Tasks;
using FirebaseAdmin.Messaging;
using System.Collections.Generic;
using System.Linq;

namespace Optsol.Components.Infra.Firebase.Messaging
{
    public class FirebaseMessagingService : IPushService
    {
        private readonly FirebaseMessaging _messaging;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public FirebaseMessagingService(ILogger<FirebaseMessagingService> logger, IMapper mapper)
        {
            _messaging = FirebaseMessaging.DefaultInstance;
            _logger = logger;
            _mapper = mapper;

            _logger?.LogInformation($"Iniciando {nameof(FirebaseMessagingService)}");
        }

        public async Task SendAsync(PushMessageAggregateRoot message)
        {
            var pushMessagesInAggregate = message.GetPushMessages().Select(push => (PushMessageBase)push);

            ValidationPushMessages(pushMessagesInAggregate);

            if (!(pushMessagesInAggregate is IEnumerable<PushMessageBase>))
            {
                _logger?.LogError($"Mensagem de tipo não suportado.Tipo esperado: { nameof(PushMessageBase)}");
                throw new Exception($"Mensagem de tipo não suportado. Tipo esperado: {nameof(PushMessageBase)}");
            }

            var pushMessages = _mapper.Map<List<Message>>(pushMessagesInAggregate);

            var response = await _messaging.SendAllAsync(pushMessages);

            _logger?.LogInformation($"Resposta: {response.ToJson()}");
        }

        private void ValidationPushMessages(IEnumerable<PushMessageBase> pushMessagesInAggregate)
        {
            foreach (var push in pushMessagesInAggregate)
            {
                push.Validate();
            }
        }
    }
}