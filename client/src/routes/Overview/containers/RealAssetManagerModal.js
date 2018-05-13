import React from 'react'
import PropTypes from 'prop-types'
import { Button, Header, Modal, Message, Form } from 'semantic-ui-react'

class RealAssetManagerModal extends React.Component {
  constructor (props) {
    super(props)
    this.state = {
      selectedExchange: null,
      error: null,
      showMessage: false
    }
  }

  onExchangeSelect = (e, { value }) => {
    if (value === this.state.selectedExchange) return
    this.setState({
      selectedExchange:value,
    })
  }

  onMirrorClick = async (currency) => {
    const error = await this.props.mirrorExchangeAssets(this.state.selectedExchange)
    if (error !== null) this.setState({ error: error })
    else this.setState({ selectedExchange: null })
    this.setState({ showMessage: true })
  }

  render () {
    const {
      selectedExchange,
      showMessage,
      error,
    } = this.state
    const {
      marketData,
      mirrorExchangeAssetsFetching,
      marketDataFetching
    } = this.props
    const possibleExchanges = marketData ? marketData.map(o => ({ key: o.id, text: o.name, value: o.id })) : []
    return (
      <Modal trigger={
        <Button
          disabled={marketDataFetching || possibleExchanges.length === 0}
          color='green'
          loading={marketDataFetching}
        >
          Manage assets
        </Button>
        } closeIcon size='mini'>
        <Header icon='money' content='Asset Management' />
        <Modal.Content>
          <div className='newStrategyModalContent'>
            <Form widths='equal' loading={mirrorExchangeAssetsFetching}>
              <Form.Group>
                <Form.Select
                  fluid
                  label='Exchange'
                  placeholder='Exchange'
                  name='exchange'
                  value={selectedExchange}
                  options={possibleExchanges}
                  onChange={this.onExchangeSelect} />
                <Form.Button
                  disabled={selectedExchange == null}
                  onClick={this.onMirrorClick}
                  color='green'
                  className='modalButton'>
                  Mirror Assets
                </Form.Button>
              </Form.Group>
            </Form>
          </div>
          <Message
            success={showMessage && !error}
            error={showMessage && error}
            hidden={!showMessage}
            header={error ? `Insufficient funds for:\n ${error}` : 'Success'}
          />
        </Modal.Content>
      </Modal>
    )
  }
}

RealAssetManagerModal.propTypes = {
  marketData: PropTypes.array,
  mirrorExchangeAssets: PropTypes.func,
  mirrorExchangeAssetsFetching: PropTypes.bool,
  marketDataFetching: PropTypes.bool,
}

export default RealAssetManagerModal
