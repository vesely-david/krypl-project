import React from 'react'
import PropTypes from 'prop-types'
import { Button, Header, Icon, Modal, Form, Message, Label } from 'semantic-ui-react'
import './styles/NewStrategyModalContent.scss'
// import { connect } from 'react-redux'
// import { bindActionCreators } from 'redux'

class NewStrategyModal extends React.Component {
  constructor (props) {
    super(props)
    this.state = {
      selectedExchange: null,
      assetCurrency: null,
      assetValueError: false,
      name: '',
      description: '',
      assetValue: '',
      assetValueMaximum: null,
      assetOptions: null,
      returnedId: null,
      allAssets: this.props.allAssets
    }
  }

  onInputChange = (e, { name, value }) => {
    this.setState({ [name]: value })
    if (name === 'assetValue') this.setState({ assetValueError: false })
  }

  onExchangeSelect = (e, { value }) => {
    if (value === this.state.selectedExchange) return
    this.setState({
      selectedExchange:value,
      assetOptions: this.state.allAssets
        .find(o => o.value === value).assets
        .filter(o => o.free > 0)
        .map(o => ({ ...o, taken: 0, available: true })),
      assetCurrency: null,
      assetValue: '',
      assetValueError: false,
      assetValueMaximum: null,
    })
  }

  onCurrencySelect = (e, { value }) => {
    this.setState({
      assetCurrency: value,
      assetValue: '',
      assetValueError: false,
      assetValueMaximum: this.state.assetOptions.find(o => o.value === value).free
    })
  }

  onAddAsset = () => {
    const { assetCurrency, assetValue, assetValueMaximum } = this.state
    if (!assetValue.match(/^(0\.\d+)$|^([1-9]\d*(\.\d+)?)$/) || parseFloat(assetValue) > assetValueMaximum) {
      this.setState({ assetValueError: true })
      return
    }
    this.setState({ assetOptions: this.state.assetOptions.map(
      o => o.value === assetCurrency ? { ...o, available: false, taken: assetValue } : o) })
    this.setState({ assetValue: '', assetCurrency: null, assetValueMaximum: null })
  }

  onRemoveAsset = (currency) => {
    this.setState({ assetOptions: this.state.assetOptions.map(
      o => o.value === currency ? { ...o, available: true, taken: 0 } : o) })
  }

  onSubmit = async () => {
    debugger;
    const { name, description, assetOptions, selectedExchange } = this.state
    const id = await this.props.registerStrategy(name, selectedExchange, description, assetOptions
      .filter(o => !o.available)
      .map(o => ({ currency: o.value, amount: o.taken })))
    this.setState({ returnedId: id })
    if (id > 0) this.cleanForm()
  }

  cleanForm = () => {
    this.setState({
      assetCurrency: null,
      name: '',
      description: '',
      selectedExchange: null,
      assetValueError: false,
      assetValueMaximum: null,
      assetValue: '',
      assetOptions: this.state.assetOptions
        ? this.state.assetOptions.map(o => ({ ...o, available: true, taken: 0 }))
        : null
    })
  }

  render () {
    const {
      name,
      description,
      assetCurrency,
      assetValue,
      assetOptions,
      assetValueError,
      allAssets,
      selectedExchange,
      assetValueMaximum,
      returnedId
    } = this.state
    const submitEnabled = name && description && assetOptions && assetOptions.find(o => !o.available)
    const possibleExchanges = allAssets.map(o => ({ key: o.value, text: o.text, value: o.value }))
    const possibleOptions = assetOptions ? assetOptions
      .filter(o => o.available).map(o => ({ key: o.value, value: o.value, text: o.text })) : []
    return (
      <Modal trigger={<Button primary>New Strategy</Button>} closeIcon onClose={this.cleanForm}>
        <Header icon='plug' content='New Strategy' />
        <Modal.Content>
          <div className='newStrategyModalContent'>
            <Form loading={this.props.registrationPending}>
              <Form.Group>
                <Form.Input
                  fluid
                  width={11}
                  value={name}
                  label='Name'
                  name='name'
                  placeholder='Name'
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
                  value={assetCurrency}
                  disabled={possibleOptions.length === 0 || selectedExchange === null}
                  options={possibleOptions}
                  placeholder='Currency'
                  onChange={this.onCurrencySelect} />
                <Form.Input
                  type='number'
                  disabled={assetCurrency === null}
                  fluid
                  error={assetValueError}
                  label={`Value${assetValueMaximum ? ` (Maximum: ${assetValueMaximum})` : ''}`}
                  value={assetValue}
                  name='assetValue'
                  onChange={this.onInputChange} />
                <Form.Button
                  disabled={assetValue === ''}
                  onClick={this.onAddAsset}color='green'
                  className='modalButton'>
                  <Icon name='checkmark' size='large' />
                </Form.Button>
              </Form.Group>
            </Form>
            <div style={{ minHeight: '30px' }}>
              {assetOptions && assetOptions.filter(o => !o.available).map(o => (
                <Label color='blue' key={o.value}>
                  {o.text}
                  <Label.Detail>{o.taken}</Label.Detail>
                  <Icon name='close' onClick={() => this.onRemoveAsset(o.value)} />
                </Label>
              ))}
            </div>
          </div>
          <Message
            onDismiss={() => this.setState({ returnedId: null })}
            success={returnedId && returnedId > 0}
            error={returnedId && returnedId <= 0}
            hidden={returnedId === null}
            header={returnedId && returnedId > 0 ? `Strategy Registered. Id: ${returnedId}` : 'Error getting id'}
          />
        </Modal.Content>
        <Modal.Actions>
          <Button disabled={!submitEnabled} color='green' onClick={() => this.onSubmit()}>
            <Icon name='checkmark' /> Register
          </Button>
        </Modal.Actions>
      </Modal>
    )
  }
}

NewStrategyModal.propTypes = {
  allAssets: PropTypes.array,
  registerStrategy: PropTypes.func,
  registrationPending: PropTypes.bool,
}

export default NewStrategyModal
