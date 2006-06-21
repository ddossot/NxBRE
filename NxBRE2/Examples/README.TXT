Command lines for running the examples:

  Login -l %1 -e %2 "C:/NxBRE/Rulefiles/login.xbre"
     where %1 is the log trace level (1-9) and %2 the error trace level (1-9)

  Discount -l %1 -e %2 "C:/NxBRE/Rulefiles/discount.xbre"
     where %1 is the log trace level (1-9) and %2 the error trace level (1-9)

  FraudControl %1 "C:\NxBRE\Rulefiles\fraudcontrol.ruleml"
     where %1 is the number of tens of dummy customers data to create (advice: use 10 or more)

  Telco %1 "C:\My Documents\Development\NxBRE\Rulefiles\telco-rating.ruleml"
     where %1 is the number of dozens of dummy calls data to create (advice: use 100 or more)
