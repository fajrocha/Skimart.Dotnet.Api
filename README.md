

# Stripe
## Running Stripe CLI
On Windows:

- Login: 

```shell
.\stripe.exe login
```

- Listen on webhook payment events:

```shell
 .\stripe.exe listen -f https://localhost:7070/api/payment/webhook -e payment_intent.succeeded,payment_intent.payment_failed
```

