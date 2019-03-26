import React from 'react'
import { Button, Header, Icon, Modal, Form, Message, Label } from 'semantic-ui-react'
import styles from '../styles/modals.module.scss';

class NewStrategyModal extends React.Component {
  constructor (props) {
    super(props)
    this.state = {
      selectedExchange: null,
      selectedCurrency: null,
      name: '',
      description: '',
      assetValue: '',
      assetValueTouched: false,
      returnedId: null,
      strategyAssets: []
    }
  }

  onInputChange = (e, { name, value }) => {
    this.setState({ [name]: value })
    if(name === 'assetValue') this.setState({assetValueTouched: true})
  }

  onExchangeSelect = (e, { value }) => {
    if (value === this.state.selectedExchange) return
    this.setState({
      selectedExchange: value,
      selectedCurrency: null,
      assetValue: '',
      assetValueTouched: false,
    })
  }

  onCurrencySelect = (e, { value }) => {
    if (value === this.state.selectedCurrency) return
    const {
      strategyAssets,
    } = this.state;
    const curr = strategyAssets.find(o => o.id === value);
    this.setState({ 
      assetValue: curr ? curr.amount : '',
      selectedCurrency: value,
      assetValueTouched: false,
    })
  }

  onAssetAdd = (currencyName) => {
    const {
      selectedCurrency,
      selectedExchange,
      assetValue,
      strategyAssets
    } = this.state
    const strategyAsset = strategyAssets.find(o => o.id === selectedCurrency);
    if(strategyAsset){
      this.setState({
        strategyAssets: strategyAssets.map(o => o.id === selectedCurrency ? ({...o, amount: assetValue}) : o),
        assetValue: '',
        assetValueTouched: false,
        selectedCurrency: null,
      })
    } else {
      this.setState({
        strategyAssets: [...strategyAssets, {id: selectedCurrency, exchange: selectedExchange, amount: assetValue, currency: currencyName}],
        assetValue: '',
        assetValueTouched: false,
        selectedCurrency: null,
      })
    }
  }

  onAssetRemove = (currency) => {
    this.setState({
      strategyAssets: this.state.strategyAssets.filter(o => o.id !== currency),
    })
  }

  onSubmit = async () => {
    const {
      name,
      description,
      selectedExchange,
      strategyAssets
    } = this.state;

    const res = await this.props.registerStrategy(
      name, 
      selectedExchange, 
      description,
      strategyAssets.filter(o => o.exchange === selectedExchange)
        .map(o => ({id: o.id, amount: Number.parseFloat(o.amount)}))
    )
    if(!res){
      alert('Error');
      return;
    }
    this.setState({ returnedId: res.value })
    this.cleanForm(false)
  }

  cleanForm = (deleteReturnedId) => {
    this.setState({
      name: '',
      description: '',
      selectedExchange: null,
      selectedCurrency: null,
      assetValue: '',
      strategyAssets: []
    })
    if (deleteReturnedId) this.setState({ returnedId: null })
  }

  render () {
    const {
      name,
      description,
      assetValue,
      selectedExchange,
      selectedCurrency,
      returnedId,
      assetValueTouched,
      strategyAssets
    } = this.state
    const {
      allAssets,
    } = this.props;

    const originAssets = selectedExchange ? allAssets.find(o => o.exchange === selectedExchange).assets : null;

    const possibleExchanges = allAssets.map(o => ({ key: o.exchange, text: o.exchange, value: o.exchange }));
    const possibleCurrencies = originAssets ? originAssets.filter(o => o.freeAssetId && o.free > 0)
      .map(o => ({ key: o.currency, text: o.currency, value: o.freeAssetId })) : [];
    const assetOrigin = originAssets ? originAssets.find(o => o.freeAssetId === selectedCurrency) : null;
    const assetValueError = (assetOrigin && assetOrigin.free < assetValue) || (assetValueTouched && !assetValue);
    return (
      <Modal
        trigger={<Button primary>New Strategy</Button>}
        closeIcon
        onClose={() => this.cleanForm(true)}
        onOpen={this.onModalOpen}>
        <Header icon='plug' content='New Strategy' />
        <Modal.Content>
          <Form loading={this.props.registrationPending}>
            <Form.Group>
              <Form.Input
                fluid
                width={11}
                value={name}
                label='Title'
                name='name'
                placeholder='Strategy Title'
                onChange={this.onInputChange} />
              <Form.Select
                fluid
                width={5}
                label='Exchange'
                placeholder='Exchange'
                name='exchange'
                value={selectedExchange}
                options={possibleExchanges}
                onChange={this.onExchangeSelect} />
            </Form.Group>
            <Form.TextArea
              label='Description'
              name='description'
              value={description}
              placeholder='Tell me more...'
              onChange={this.onInputChange} />
            <Form.Group widths='equal'>
              <Form.Select
                fluid
                label='Currency'
                name='assetCurrency'
                value={assetOrigin ? assetOrigin.id : null}
                disabled={possibleCurrencies.length === 0 || selectedExchange === null}
                options={possibleCurrencies}
                placeholder='Currency'
                onChange={this.onCurrencySelect} />
              <Form.Input
                type='number'
                disabled={selectedCurrency === null}
                fluid
                error={assetValueError}
                label={`Value${assetOrigin ? ` (Maximum: ${assetOrigin.free}` : ''})`}
                value={assetValue}
                name='assetValue'
                onChange={this.onInputChange} />
              <Form.Button
                disabled={assetValue === '' || assetValueError}
                onClick={() => this.onAssetAdd(assetOrigin.currency)}
                color='green'
                className={styles.form_button}
              >
                <Icon name='checkmark' size='large' />
              </Form.Button>
            </Form.Group>
          </Form>
          <div className={styles.assets_wrapper}>
            {strategyAssets.map(curr => {
                return (
                  <Label key={curr.currency} onClick={() => this.onCurrencySelect(null, {value: curr.id})}>
                    {`${curr.currency} ${curr.amount}`}
                    <Icon name='close' onClick={(e) => {
                      e.stopPropagation();
                      this.onAssetRemove(curr.id);
                    }} />
                  </Label>)
              })}
          </div>
          <Message
            onDismiss={() => this.setState({ returnedId: null })}
            success={returnedId !== null}
            error={returnedId === null}
            hidden={returnedId === null}
            header={returnedId ? `Strategy Registered. Id: ${returnedId}` : 'Error getting id'}
          />
        </Modal.Content>
        <Modal.Actions>
          <Button disabled={strategyAssets.length === 0 || !name} color='green' onClick={() => this.onSubmit()}>
            <Icon name='checkmark' /> Register
          </Button>
        </Modal.Actions>
      </Modal>
    )
  }
}

export default NewStrategyModal
