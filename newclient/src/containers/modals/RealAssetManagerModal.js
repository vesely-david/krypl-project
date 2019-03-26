import React, { useState } from 'react'
import { Button, Header, Modal, Message, Form } from 'semantic-ui-react'
import styles from '../styles/modals.module.scss';

export default ({
  color,
  marketDataFetching,
  exchanges,
  submitAssets
}) => {
  const [modalOpened, setModalOpen] = useState(false);
  const [isSubmitting, setSubmitting] = useState(false);
  const [error, setError] = useState(false);
  const [selectedExchange, setSelectedExchange] = useState(null);

  const onMirrorClick = async () => {
    setSubmitting(true);
    const result = await submitAssets(selectedExchange)
    setSubmitting(false);
    if(!result) setError(true);
    else{
      setSelectedExchange(null);
      setModalOpen(false);
    }
  }
  const possibleExchanges = exchanges.filter(o => o.apiKey !== null)
    .map(o => ({ key: o.exchangeId, text: o.exchangeName, value: o.exchangeId }));

  return (
    <Modal 
      trigger={
        <Button
          disabled={marketDataFetching || possibleExchanges.length === 0}
          color={color}
          loading={marketDataFetching}
          onClick={() => setModalOpen(true)}
        >
          Manage assets
        </Button>} 
      closeIcon
      size='mini'
      open={modalOpened}
      onClose={() => setModalOpen(false)}      
    >
      <Header icon='money' content='Asset Management' />
      <Modal.Content>
        <div>
          <Form widths='equal' loading={isSubmitting}>
            <Form.Group>
              <Form.Select
                fluid
                label='Exchange'
                placeholder='Exchange'
                name='exchange'
                value={selectedExchange}
                options={possibleExchanges}
                onChange={(e, { value }) => setSelectedExchange(value)} />
              <Form.Button
                disabled={selectedExchange == null || isSubmitting}
                onClick={onMirrorClick}
                loading={isSubmitting}
                color='green'
                className={styles.form_button}
              >
                Mirror Assets
              </Form.Button>
            </Form.Group>
          </Form>
        </div>
        <Message
          error
          hidden={!error}
          header='Error during communication'
        />
      </Modal.Content>
    </Modal>
  )
}
