from falsy.falsy import FALSY


def main():
    f = FALSY()   #you need create the dir called static before you run
    f.swagger('calculatorAPI.yaml', ui=True, theme='impress') #impress theme is the responsive swagger ui, or you can use 'normal' here
    api = f.api
    return api


api = main()
