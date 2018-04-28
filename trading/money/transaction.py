from trading.money.contract import Contract


class Transaction:
    SELL = "SELL"
    BUY = "BUY"

    @staticmethod
    def _new(_type, pair, timestamp, amount, price, fee):
        return {
            'type': _type,
            'pair': pair,
            'timestamp': timestamp,
            'amount': amount,
            'price': price,
            'fee': fee
        }

    @classmethod
    def sell(cls, pair, timestamp, amount, price, fee):
        return Transaction._new(cls.SELL, pair, timestamp, amount, price, fee)

    @classmethod
    def buy(cls, pair, timestamp, amount, price, fee):
        return Transaction._new(cls.BUY, pair, timestamp, amount, price, fee)

    @classmethod
    def gained_contract(cls, transaction):
        _type = transaction['type']
        pair = transaction['pair']
        if _type == cls.SELL:
            return Contract.new(pair['priceContract'], transaction['amount'] * transaction['price'])
        elif _type == cls.BUY:
            return Contract.new(pair['tradeContract'], transaction['amount'])

    @classmethod
    def subtracted_contract(cls, transaction):
        _type = transaction['type']
        pair = transaction['pair']
        if _type == cls.SELL:
            return Contract.new(pair['tradeContract'], transaction['amount'])
        elif _type == cls.BUY:
            return Contract.new(pair['priceContract'], transaction['amount'] * transaction['price'])
